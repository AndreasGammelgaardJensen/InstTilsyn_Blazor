using System.Text;
using iTextSharp.text.pdf.parser;
using iTextSharp.text.pdf;
using PDFService.Tags;
using PDFExtractionLib;

string pdfUrl = "https://iwtilsynpdf.kk.dk/pdf/2236.pdf"; // Replace with the actual PDF URL
string savePath = "2236.pdf"; // Replace with the desired save path
//string savePath = "1679.pdf";
//string savePath = "6030.pdf";
//string savePath = "downloaded_file.pdf";

using (HttpClient client = new HttpClient())
{
    try
    {
        HttpResponseMessage response = await client.GetAsync(pdfUrl);

        if (response.IsSuccessStatusCode)
        {
            using (var contentStream = await response.Content.ReadAsStreamAsync())


            using (var fileStream = System.IO.File.Create(savePath))
            {
                await contentStream.CopyToAsync(fileStream);
            }

            Console.WriteLine("PDF file downloaded successfully!");
        }
        else
        {
            Console.WriteLine($"Failed to download PDF. Status code: {response.StatusCode}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}

Executer executer = new Executer();

executer.Execute(new HttpClient(), savePath, pdfUrl);



//PdfReader reader = new PdfReader(savePath);
//int intPageNum = reader.NumberOfPages;
//string[] words;
//string line;

//for (int i = 1; i <= intPageNum; i++)
//{
//    var text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

//    //Check if page contains

//    Categories categories = Tags.Evaluate_text_contains_categori(text);

//    if (categories == Categories.None) continue;
//    words = text.Split('\n');
//    for (int j = 0, len = words.Length; j < len; j++)
//    {
//        line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j]));


//        if (line.Contains(Tags.Section_Vurdering) || line.Contains(Tags.Section_Vurdering_v2))
//        {
//            var lineArray = line.Trim().Split(' ');
//            var indsats = string.Join(" ", lineArray[lineArray.Length - 2], lineArray[lineArray.Length - 1]);
//            Console.WriteLine(Tags.TagConverter(categories));

//            Console.WriteLine(indsats);
//        }
//        if (line.Contains(Tags.Section_Indsats))
//        {
//            line = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(words[j+1]));
//            var lineArray = line.Trim().Split(' ');
//            var indsats = string.Join(" ", lineArray[lineArray.Length - 2], lineArray[lineArray.Length - 1]);
//            Console.WriteLine(Tags.TagConverter(categories));

//            Console.WriteLine(indsats);
//        }
//    }
//}

//Console.ReadLine();

