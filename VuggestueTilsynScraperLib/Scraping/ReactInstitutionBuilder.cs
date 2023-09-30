using Microsoft.VisualBasic;
using ModelsLib.Models;
using OpenQA.Selenium;
using Serilog;
using System.Collections.ObjectModel;
using System.Transactions;
using VuggestueTilsynScraperLib.Interfaces;
using VuggestueTilsynScraperLib.Security;

namespace VuggestueTilsynScraperLib.Scraping
{
    public class ReactInstitutionBuilder : IInstitutionBuilder
    {
        private IWebDriver _webDriver { get; }
        private readonly ILogger _logger;
        public ReactInstitutionBuilder(IWebDriver webDriver, ILogger logger)
        {
            _webDriver = webDriver;
            _logger = logger;
        }

        public Address BuildAdress()
        {
            try
            {
                var add = _webDriver.FindElement(By.TagName("address"));

                //Console.WriteLine(add.Text);
                return Address.ConvertStringToAddress(add.Text);
            }
            catch(Exception e)
            {
                throw new Exception("Adress could not be scraped");
            }

        }

        public InstitutionFrontPageModel BuildBaseInformation()
        {
            string name = "";
            string homePage = "";
            string institutionType = "";
            string profile = "";

            try
            {
                try
                {
                //Select name
                    IWebElement contentPanel = _webDriver.FindElement(By.ClassName("c-content-cell-1"));

                    name = contentPanel.FindElement(By.TagName("h2")).Text;

                }catch(Exception e)
                {
                    throw new Exception("Could not select Name");
                }
                //Console.WriteLine(name.Text);

                //Select Home page
                ///Select first homepage
                try
                {
                    IWebElement infoContent = _webDriver.FindElements(By.CssSelector(".c-data-list")).First();
                    ReadOnlyCollection<IWebElement> info = infoContent.FindElements(By.CssSelector(".c-data-list > dd > a"));
                    homePage = info.Last().GetAttribute("href");
                        //Console.WriteLine(homePage);
                }catch (Exception e)
                {
                    _logger.Warning("Could not Select homePage");
                }


                try
                {
                    //Select Instutution type
                    institutionType = _webDriver.FindElement(By.CssSelector("div.c-category-badge")).Text;
                }
                catch (Exception e)
                {
                    throw new Exception("Could not Select institution type");
                }


                try
                {
                    //Select Profile
                    IWebElement element = _webDriver.FindElement(By.XPath("//dd[contains(., 'Profil')]"));
                    profile = element.Text.Split("\r")[1].Replace("\n","");
                }
                catch (Exception e)
                {
                    _logger.Information("Could not Select profile");
                }

                return new InstitutionFrontPageModel
                {
                    Name = name,
                    TypeEnum = InstitutionFrontPageModel.InstitutionTypeTranstator(institutionType),
                    HomePage = homePage,
                    profile = profile,
                };

            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Base information could not be retreved: {0}",e.Message));
            }
           
        }

        public Pladser BuildPladser()
        {
            try
            {
                //Select waiting list
                //TODO: Håndter denn: Flerbørnsdagplejen Delfinen
                var infoContentPladser = _webDriver.FindElements(By.CssSelector(".c-data-list > dd")).Where(x => x.Text.Contains("Vuggestue") || x.Text.Contains("Børnehave") || x.Text.Contains("Dagpleje"));

                int børnehave = 0;
                int vuggestue = 0;
                int dagpleje = 0;

                foreach (var el in infoContentPladser)
                {
                    var test = el.Text.Split("\n");

                    foreach (var t in test)
                    {
                        t.Trim();
                        if (t.Contains("Børnehave"))
                        {
                            børnehave = int.Parse(t.Split(":")[1].Trim().Split(" ").First().Trim());
                        }
                        else if (t.Contains("Vuggestue"))
                        {
                            vuggestue = int.Parse(t.Split(":")[1].Trim().Split(" ").First().Trim());

                        }
                        else if (t.Contains("Dagpleje"))
                        {
                            dagpleje = int.Parse(t.Split(":")[1].Trim().Split(" ").First().Trim());
                        }
                    }
                }

                return new Pladser
                {
                    BoernehavePladser = børnehave,
                    VuggestuePladser = vuggestue,
                    DagplejePladser = dagpleje
                };
            }
            catch (Exception e)
            {
                _logger.Information("Could not be scraped");
                return null;
            }
        }

        public InstitutionTilsynsRapport BuildInstitutionTilsynsRapport()
        {
            ////Select report url
            try
            {
                IWebElement element = _webDriver.FindElement(By.XPath("//a[contains(., 'Se rapport')]"));
                var href = element.GetAttribute("href");
                //Console.WriteLine(element.Text);

                GetFileType(element.Text);

                return new InstitutionTilsynsRapport
                {
                    Id = Guid.NewGuid(),
                    fileUrl = href,
                    documentType = GetFileType(href),
                    copyDate = DateTime.Now,
                    Name = href,
                    hash = HashGenerator.GetSHA256Hash(href)
                };

            }
            catch(Exception e) 
            {
                _logger.Information("Contains no report");
                return new InstitutionTilsynsRapport();
            }
        }

        public InstKoordinates BuildInstitutionCoordinates()
        {
            return new InstKoordinates
            {
                lat = null,
                lng = null,
            };
        }

        private string GetFileType(string url)
        {
            var urlList = url.Split(".");

            return urlList.Last();
        }
    }
}