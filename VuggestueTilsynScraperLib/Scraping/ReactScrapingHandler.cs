using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System.Collections.ObjectModel;
using ModelsLib.DatabaseModels;
using DataAccess.Interfaces;
using ModelsLib.Models;
using Serilog;
using OpenQA.Selenium.Edge;

namespace VuggestueTilsynScraperLib.Scraping
{
    public class ReactScrapingHandler
    {
        private static readonly string _u = "https://www.kk.dk/borger/pasning-og-skole/pasning-0-6-aar/skriv-op-til-vuggestue-og-dagpleje/se-vuggestuer-og-dagplejere";
        private static IInstitutionRepository _institutionRepository;
        private readonly IQueryable<InstitutionFrontPageModelDatabasemodel> _institutionDatabaseModels;
        private readonly ILogger _logger;

        public ReactScrapingHandler(IInstitutionRepository institutionRepository, IQueryable<InstitutionFrontPageModelDatabasemodel> institutionDatabaseModels, ILogger logger)
        {
            _institutionRepository = institutionRepository;
            _institutionDatabaseModels = institutionDatabaseModels;
            _logger = logger;
        }

        public async void Handle(Action<Guid, List<InstitutionTilsynsRapport>> test, bool runHeadless = false)
        {
            IWebDriver d = InitializeHeadless(_u, _logger,  runHeadless);
            InstitutionDirector director = new InstitutionDirector();
            ReactInstitutionBuilder builder = new ReactInstitutionBuilder(d, _logger);
            director.Builder = builder;

            _logger.Information("IAM HANDLING");



            Thread.Sleep(2000);
            AcceptCookie(d);
            LoadMoreInstitutionsButtonClick(d, _logger);

            //webElement.Click();
            Thread.Sleep(2000);
            var liElements2 = SelectInstitutionElementsToScrape(d, _logger);




            var counter = 0;
            CountingMap countingMap = new CountingMap();
            countingMap.CreateCountingMap(liElements2);

            foreach (var firstElement in liElements2)
            {
                _logger.Information($"Counter {counter}");
				try
				{
                //Handle No such element exceprion
                    try { firstElement.Click(); } catch { _logger.Information("Missing element, skipping to next element"); continue; }

                        var institution = director.GetInstitutionFrontPageModel();

                        //Validate institution
                        if (!Validation.Validation.ValidateInstitutionModel(institution)) {
                            GoBackToPreveusListPage(d);
                        };

                        Guid institutionId;
                        //TODO: Validate by hash of an institution. It should only update in adress name and pladser have updated.
                        if (_institutionDatabaseModels.Any(x => x.Name == institution.Name))
                        {
                            institutionId = _institutionRepository.UpdateInstitution(institution);
                        }
                        else
                        {
                            institutionId = _institutionRepository.AddInstitution(institution);
                        }

                        var institutionDBmodel = _institutionRepository.GetInstitutionById(institutionId);

                        if (institutionDBmodel.InstitutionTilsynsRapports.Any())
                            //TODO: Ensure that report gets the correct report ID. We have to do better Id generation
                            //Get a query with all the reports
                            test(institutionDBmodel.Id, institutionDBmodel.InstitutionTilsynsRapports);


                        GoBackToPreveusListPage(d);
                        counter++;
                        countingMap.SetCounting(institution.Name);
                        _logger.Information($"{counter}/{liElements2.Count()} is scraped");
                    
                    }

                    //TODO: Make proper error handling so that we will store relevant errors to the db
                    catch (Exception e) 
                    {
                        try
                        {
						    _logger.Error(string.Format("{0} could not be scraped: Error: {1}", firstElement.Text, e.Message));

					    }
					    catch (WebDriverException)
                        {
						    _logger.Error(string.Format("Could not be scraped: Error: {0}", e.Message));
					    }
					   
                        try
                        {
                            GoBackToPreveusListPage(d);

                        }
                        catch (OpenQA.Selenium.NoSuchElementException selNo)
                        {
                            continue;
                        }

					//TODO: Do some logging and store it to db
					_logger.Error("Inner exception: {innerException}", e.InnerException);
				}
            }
            Console.WriteLine(string.Format("{0} elements retrived",counter));
            d.Quit();
        }

        private static void GoBackToPreveusListPage(IWebDriver d)
        {
            //Scroll to the top of the page, wherethe back button is located
            string js = string.Format("window.scroll(0, 0);");
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)d;
            jsExecutor.ExecuteScript(js);
            IWebElement backToList = d.FindElement(By.ClassName("c-go-back"));
            backToList.Click();


            //TODO: Ensure that the google map is zoomed out, because when navigating back the map will be zoomed to area where the current institution was located
            Thread.Sleep(1000);
            IWebElement googleZoomOut = d.FindElement(By.CssSelector("button.ol-zoom-out"));
            for (int i = 0; i < 5; i++)
            {
                googleZoomOut.Click();
                Thread.Sleep(500);
            }


            IWebElement webElement2 = d.FindElement(By.CssSelector("button.widget-table-loadmore-button"));
            webElement2.Click();
        }


        private static IWebDriver InitializeHeadless(string baseUrl, ILogger logger, bool runHeadless = false)
        {
            // Find and click cookie
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Add the headless argument to run Chrome in headless mode.
            options.AddArgument("--disable-gpu");
            //options.AddArguments("--disable-dev-shm-usage");
            options.AddArguments("--no-sandbox");
            options.AddArgument("--whitelisted-ips=''");
            //options.AddArgument("--ignore-certificate-errors");
            //options.AddArgument("--remote-debugging-port=9222");

			new DriverManager().SetUpDriver(new ChromeConfig());
            IWebDriver d = runHeadless ? new ChromeDriver(options) : new ChromeDriver();

            d.Navigate().GoToUrl(baseUrl);
            logger.Information("Launching url: {baseUrl}", baseUrl);

            return d;
        }

        private static void AcceptCookie(IWebDriver d)
        {
            // Find and click cookie
            IWebElement AcceptElement = d.FindElement(By.CssSelector("button.agree-button.eu-cookie-compliance-default-button"));

            if (AcceptElement != null)
            {
                AcceptElement.Click();
            }

            Thread.Sleep(2000);
        }

        private static ReadOnlyCollection<IWebElement> SelectInstitutionElementsToScrape(IWebDriver d, ILogger logger)
        {
            // Find and click cookie
            ReadOnlyCollection<IWebElement> liElements2 = d.FindElements(By.CssSelector("#widget-tabpanel-list > div > ul > li"));

            logger.Information($"{liElements2.Count()}");

            return liElements2;
        }

        private static void LoadMoreInstitutionsButtonClick(IWebDriver d, ILogger logger)
        {
            // Find and click cookie
            var loadMoreIsClicked = false;

            do
            {
                try
                {
                    IWebElement webElement = d.FindElement(By.CssSelector("button.widget-table-loadmore-button"));
                    webElement.Click();
                    loadMoreIsClicked = true;
                    Thread.Sleep(2000);
                    logger.Information("Load more button is clicked");
                }
                catch (Exception e)
                {
                    loadMoreIsClicked = true;
                    logger.Error("Load more button does not yet exist");
                }


            } while (!loadMoreIsClicked);
            //Click load more buttons
        }

        private class CountingMap
        {
            private Dictionary<string, bool> countingMap = new Dictionary<string, bool>();

            public int GetCounted 
            {
                get { return countingMap.Where(x => x.Value == true).Count(); }            
            }

            public string GetCountedToString
            {
                get { 
                    var counted =  countingMap.Where(x => x.Value == true).Count();
                    return string.Format("{0}/{1} is scraped", counted, countingMap.Count());
                
                }
            }

            public Dictionary<string, bool> CreateCountingMap(ReadOnlyCollection<IWebElement> institutions)
            {
                try {  
                    foreach (var firstElement in institutions)
                    {
                        var bla = firstElement.Text;
                        var test = firstElement.Text.Split('-');
                        var instName = firstElement.Text.Split('-')[0].Trim();
                        countingMap.Add(firstElement.Text.Trim(), false);
                    }
                }
                catch (ArgumentException ae) { Log.Error(ae.StackTrace); }
                return countingMap;

            }

            public void SetCounting(string instName)
            {
                var key = countingMap.FirstOrDefault(x => x.Key.Contains(instName)).Key;
                countingMap[key] = true;
            }

            
        }

       
    }
}   
