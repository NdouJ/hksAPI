using hksAPI.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;
using System.Text;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HksController : ControllerBase
    {
        Breeder breeder;

        public HksController()
        {
            breeder = new Breeder();
        }

        [HttpGet]
        public IActionResult GetBreeders()
        {
            string localPdfPath = DownloadPdf("https://hks.hr/wp-content/uploads/2023/11/2023-11_06-LEGLA-1.pdf");
            if (localPdfPath != null)
            {
                PdfDocument pdfDocument = new PdfDocument(new PdfReader(localPdfPath));
                ExtractTextFromPdf(pdfDocument);
                return Ok(breeder);
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
                    // Get the file name from the URL and save the PDF locally in the "App_Data\dog" directory
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
                // Handle the exception or log it
                // You can add code here to log or display the exception details
                // For simplicity, we'll return null when the download fails
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

            // Process the extracted text and populate the 'breeder' object
            // Modify this part as needed based on the structure of the PDF

            // Example: breeder.Name = extractedText.ToString();
        }
    }
}
