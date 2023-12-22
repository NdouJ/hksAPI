using hksAPI.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HksController : ControllerBase
    {
        Breeder breeder;
        List<BreederPack> breederPacks = new List<BreederPack>();
       // const string url = "https://hks.hr/wp-content/uploads/2023/11/2023-11_06-LEGLA-1.pdf"; 

        public HksController()
        {
            breeder = new Breeder();
        }

        [HttpGet("CheckOib")]
        public IActionResult CheckOib(string oib)
        {
            if (string.IsNullOrEmpty(oib) || !Regex.IsMatch(oib, "^[0-9]{11}$"))
                return BadRequest("Invalid OIB format.");

            var oibSpan = oib.AsSpan();
            var a = 10;
            for (var i = 0; i < 10; i++)
            {
                if (!int.TryParse(oibSpan.Slice(i, 1), out int number))
                    return BadRequest("Invalid OIB format.");

                a += number;
                a %= 10;

                if (a == 0)
                    a = 10;

                a *= 2;
                a %= 11;
            }

            var kontrolni = 11 - a;

            if (kontrolni == 10)
                kontrolni = 0;

            bool isValid = kontrolni == int.Parse(oibSpan.Slice(10, 1));

            return isValid ? Ok("OIB is valid.") : BadRequest("Invalid OIB.");
        }



        [HttpGet]
        public IActionResult GetBreeders(string url )
        {
            string localPdfPath = DownloadPdf(url);
            if (localPdfPath != null)
            {
                PdfDocument pdfDocument = new PdfDocument(new PdfReader(localPdfPath));
                ExtractTextFromPdf(pdfDocument);
                return Ok(breederPacks);
            }
            else
            {
                return NotFound("Failed to download the PDF.");
            }
        }


       private string DownloadPdf(string pdfUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = Path.GetFileName(pdfUrl);
                    string localDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "dog");
                    Directory.CreateDirectory(localDirectory); // Ensure the directory exists

                    string localPdfPath = Path.Combine(localDirectory, fileName);

                    client.DownloadFile(pdfUrl, localPdfPath);
                    return localPdfPath;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private void ExtractTextFromPdf(PdfDocument pdfDocument)
        {
            StringBuilder extractedText = new StringBuilder();
            for (int pageNum = 1; pageNum <= pdfDocument.GetNumberOfPages(); pageNum++)
            {
                PdfPage page = pdfDocument.GetPage(pageNum);
                ITextExtractionStrategy strategy = new LocationTextExtractionStrategy();
                string pageText = PdfTextExtractor.GetTextFromPage(page, strategy);
                extractedText.AppendLine(pageText);
            }
            string[] lines = extractedText.ToString().Split('\n');
            List<BreederPack> breederPacks = ExstractPage(lines);
            string s = extractedText.ToString();
        }

        private List<BreederPack> ExstractPage(string[] lines)
        {
          
           // var breederPacks = new List<BreederPack>();
            foreach (string line in lines)
            {
                BreederPack b = new BreederPack();
                if (!line.StartsWith("POPIS") && !line.StartsWith("PASMINA") && !line.StartsWith("Referentica"))
                {
                    string[] words = line.Split(' ');

                    for  (int  i = 0;  i< words.Count(); i++)
                    {
                        if (double.TryParse(words[i], out _))
                        {
                            b.Pack.BirtDate = words[i];
                            b.Pack.Male = int.Parse(words[i+1]);
                            b.Pack.FMale = int.Parse(words[i+2]);
                            b.Breeder.BreederInfo = string.Join(" ", words.Skip(i + 3));
                            break;
                        }

                        if (!string.IsNullOrEmpty(b.Pack.BreedName))
                        {
                            b.Pack.BreedName += " ";
                        }

                        b.Pack.BreedName += words[i];

                      
                    }

                    if (!String.IsNullOrEmpty(b.Pack.BreedName))
                    {
                        breederPacks.Add(b);
                    }
                }
               
            }

            return breederPacks;
        }
    }
}
