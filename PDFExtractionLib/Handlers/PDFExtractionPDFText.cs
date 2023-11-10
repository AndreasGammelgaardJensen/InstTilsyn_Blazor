using System.Text;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using PDFExtractionLib.Tags;
using Serilog;

namespace PDFExtractionLib.Handlers
{
    public class PDFExtractionPDFText : IPDFExtraction
    {
        private readonly ILogger _logger;

        public PDFExtractionPDFText(ILogger logger)
        {
            _logger = logger;
        }

        public Dictionary<string, string> Extract_Categories(string filePath)
        {
            Dictionary<string, string> indsatsDict = new Dictionary<string, string>();

            _logger?.Information("Reading from: {filePath}", filePath);
            PdfReader reader = new PdfReader(filePath);
            int intPageNum = reader.NumberOfPages;
            string[] words;
            string line;

            for (int i = 1; i <= intPageNum; i++)
            {
                var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                //Check if page contains
                Categories categories = Tags.Tags.Evaluate_text_contains_categori(text);

                if (categories == Categories.None) continue;
                words = text.Split('\n');
                for (int j = 0, len = words.Length; j < len; j++)
                {
                    line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j])).Trim();

                    try
                    {
                        if (line.Contains(Tags.Tags.Section_Vurdering) || line.Contains(Tags.Tags.Section_Vurdering_v2))
                        {
                            var lineArray = line.Trim().Split(' ');
                            var indsats = string.Join(" ", lineArray[lineArray.Length - 2], lineArray[lineArray.Length - 1]);
                            indsats = indsats.Replace(".", "");
                            _logger?.Information(Tags.Tags.TagConverter(categories));

                            _logger?.Information(indsats);

                            if(Tags.Tags.Validate_Indsats(indsats))
                                indsatsDict.Add(Tags.Tags.TagConverter(categories), indsats);

                        }
                        else if (line.Contains(Tags.Tags.Section_Indsats))
                        {
                            line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j + 1]));
                            var lineArray = line.Trim().Split(' ');
                            var indsats = string.Join(" ", lineArray[lineArray.Length - 2], lineArray[lineArray.Length - 1]);
                            indsats = indsats.Replace(".", "");
                            _logger?.Information(Tags.Tags.TagConverter(categories));

                            _logger?.Information(indsats);

                            if (Tags.Tags.Validate_Indsats(indsats))
                                indsatsDict.Add(Tags.Tags.TagConverter(categories), indsats);
                        }
                    }catch(IndexOutOfRangeException e)
                    {
                        _logger?.Warning("Index was out of bound: {error}", Tags.Tags.TagConverter(categories));
                    }catch(System.ArgumentException e) {

                        _logger?.Warning(e.Message);
                        indsatsDict.TryGetValue(Tags.Tags.TagConverter(categories), out var res);
                        _logger?.Warning(res);
                    }
                }
            }
            return indsatsDict;
        }

        public void Test()
        {
            Console.WriteLine("TEST EXTRACTOR");
        }
    }
}
