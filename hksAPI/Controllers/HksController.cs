using hksAPI.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HksController : ControllerBase
    {
        Breeder breeder;
        static List<BreederPack> breederPacks = new List<BreederPack>();
        // const string url = "https://hks.hr/wp-content/uploads/2023/11/2023-11_06-LEGLA-1.pdf"; 

        public HksController()
        {
            breeder = new Breeder();
        }




        [HttpGet]
        //   [Authorize]
        public IActionResult GetBreeders(string url)
        {
            string localPdfPath = DownloadPdf(url);
            try
            {
                if (localPdfPath != null)
                {
                    using (PdfDocument pdfDocument = new PdfDocument(new PdfReader(localPdfPath)))
                    {
                        ExtractTextFromPdf(pdfDocument);
 
                            SaveBreedersToDatabase(breederPacks);

                    
                        return Ok(breederPacks);
                    }
                }
                else
                {
                    return NotFound("Failed to download the PDF.");
                }
            }
            finally
            {
                // Delete the file in the finally block
                DeleteFile(localPdfPath);
            }
        }

        private void SaveBreedersToDatabase(List<BreederPack> breederPacks)
        {
            string serverName = "DESKTOP-N1K2Q5F\\SQLEXPRESS2023";
            string databaseName = "PawProtector";
            string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var uniqueBreeds = breederPacks
                     .Select(x => x.Pack.BreedName)
                     .Distinct()
                     .ToList();
                CheckAndSaveDogBreeds(connection, uniqueBreeds);

                foreach (var breederPack in breederPacks)
                {
                    SaveBreeder(connection, breederPack.Breeder);
                 
                    CheckAndSavePack(connection, breederPack);               
                }
               
            }

        }

        private void CheckAndSavePack(SqlConnection connection, BreederPack breederPack)
        {
            int breedID = GetBreedId(connection, breederPack.Pack.BreedName);
            int breederID = GetBreederId(connection, breederPack.Breeder.BreederName);

            string query = "INSERT INTO Pack (BreedNameID, BirtDate, Male, FMale, BreederID) VALUES (@BreedNameID, @BirtDate, @Male, @FMale, @BreederID)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BreedNameID", breedID);

                // Check if the date string is not empty or null
                if (!string.IsNullOrEmpty(breederPack.Pack.BirtDate))
                {
                    // Specify the date format for parsing
                    DateTime birtDate;
                    if (DateTime.TryParseExact(breederPack.Pack.BirtDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out birtDate))
                    {
                        command.Parameters.AddWithValue("@BirtDate", birtDate);
                    }
                    else
                    {
                        // Handle invalid date format
                        throw new InvalidOperationException("Invalid date format: " + breederPack.Pack.BirtDate);
                    }
                }
                else
                {
                    // Use '01012000' as the default date if it is empty or null
                    DateTime defaultDate = DateTime.ParseExact("01012000", "ddMMyyyy", CultureInfo.InvariantCulture);
                    command.Parameters.AddWithValue("@BirtDate", defaultDate);
                }

                command.Parameters.AddWithValue("@Male", breederPack.Pack.Male);
                command.Parameters.AddWithValue("@FMale", breederPack.Pack.FMale);
                command.Parameters.AddWithValue("@BreederID", breederID);

                command.ExecuteNonQuery();
            }
        }





        private int GetBreederId(SqlConnection connection, string breederName)
        {
            string query = "SELECT idBreeder FROM Breeder WHERE TRIM(UPPER(BreederName)) = TRIM(UPPER(@BreederName))";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BreederName", breederName.ToUpper().Trim());

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    throw new InvalidOperationException("Breeder not found in the database.");
                }
            }
        }



        private int GetBreedId(SqlConnection connection, string breedName)
        {
            string query = "SELECT idDog FROM Dog WHERE TRIM(UPPER(BreedName)) = TRIM(UPPER(@BreedName))";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BreedName", breedName.ToUpper().Trim());

                object result = command.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    // Handle the case where the breed name is not found
                    // You might throw an exception or return a specific value depending on your use case
                    throw new InvalidOperationException("Breed not found in the database.");
                }
            }
        }


        private void CheckAndSaveDogBreeds(SqlConnection connection, List<string> uniqueBreeds)
        {
            foreach (string breed in uniqueBreeds)
            {
                try
                {
                    if (!DogBreedExists(connection, breed.ToUpper()))
                    {
                        InsertDog(connection, breed, 0, 0, "", "");
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private bool DogBreedExists(SqlConnection connection, string breedName)
        {
            string query = "SELECT COUNT(*) FROM Dog WHERE TRIM(UPPER(BreedName))= TRIM(UPPER(@BreedName))";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BreedName", breedName.ToUpper().Trim());

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

    

    static void InsertDog(SqlConnection connection, string breedName, int avgWeightFemale, int avgWeightMale, string description, string image)
        {
            string insertQuery = "INSERT INTO Dog (BreedName, avgWeightFemale, avgWeightMale, description, image) VALUES (@BreedName, @AvgWeightFemale, @AvgWeightMale, @Description, @Image)";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@BreedName", breedName.ToUpper());
                command.Parameters.AddWithValue("@AvgWeightFemale", avgWeightFemale);
                command.Parameters.AddWithValue("@AvgWeightMale", avgWeightMale);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@Image", image);

                command.ExecuteNonQuery();
            }
        }

        private void SaveBreeder(SqlConnection connection, Breeder breeder)
        {

            if (!BreederExist(connection, breeder.BreederName.ToUpper()))
            {
                InsertBreeder(connection, breeder);
            }
        }

        private bool BreederExist(SqlConnection connection, string breederName)
        {
            string query = "SELECT COUNT(*) FROM Breeder WHERE TRIM(UPPER(BreederName))= TRIM(UPPER(@BreederName))";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@BreederName", breederName.ToUpper().Trim());

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void InsertBreeder(SqlConnection connection, Breeder breeder)
        {

            string insertQuery = "INSERT INTO Breeder (BreederName, BreederContact) VALUES (@BreederName, @BreederContact)";

            using (SqlCommand command = new SqlCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@BreederName", breeder.BreederName.ToUpper());
                command.Parameters.AddWithValue("@BreederContact", breeder.BreederContact);

                command.ExecuteNonQuery();
            }
        }

        private void DeleteFile(string filePath)
        {
            //try
            //{
            if (System.IO.File.Exists(filePath)) // Use the full namespace to avoid conflicts
            {
                System.IO.File.Delete(filePath);
            }
            // }
            //catch (Exception ex)
            //{
            // Handle or log any exceptions that may occur during file deletion
            //    Console.WriteLine($"Error deleting file: {ex.Message}");
            // }
        }


        private string DownloadPdf(string pdfUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    string fileName = Path.GetFileName(pdfUrl);
                    string localDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFiles"); // Change directory name
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
                line.Trim();
                BreederPack b = new BreederPack();
                //AIREDALE TERIJER 13.06.2023 2 5 VUKOBRATOVIĆ MARINKO KORENICA 097/671 19 89
                if (!line.StartsWith("POPIS") && !line.StartsWith("PASMINA") && !line.StartsWith("Referentica"))
                {
                    string[] words = line.Split(' ');

                    for (int i = 0; i < words.Count(); i++)
                    {

                        if (DateTime.TryParseExact(words[i], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))

                        {
                            b.Pack.BirtDate = words[i];
                            b.Pack.Male = int.Parse(words[i + 1]);
                            b.Pack.FMale = int.Parse(words[i + 2]);
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


        [HttpGet("GetBreeds")]
        [Authorize]
        public IActionResult GetBreeds()
        {
            var uniqueBreeds = breederPacks
                .Select(x => x.Pack.BreedName)
                .Distinct()
                .ToList();


            return Ok(uniqueBreeds);
        }
    }
}