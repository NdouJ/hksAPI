using hksAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace hksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
            //your logic for login process
            //If login usrename and password are correct then proceed to generate token


            //TEMP LOGIN START

            // Replace these values with your actual database connection details
            string serverName = "DESKTOP-N1K2Q5F\\SQLEXPRESS2023";
            string databaseName = "PawProtector"; // Replace with your actual database name
            string connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True;TrustServerCertificate=True;";

            string providedEmail = loginRequest.Username;
            string providedPasswordHash = loginRequest.Password;

            // Set up the SQL query with parameters
            string sqlQuery = @"
            DECLARE @ProvidedEmail NVARCHAR(255) = @Email;
            DECLARE @ProvidedPasswordHash NVARCHAR(255) = @PasswordHash;

            SELECT
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM [User]
                    WHERE username = @ProvidedEmail AND passwordHash = @ProvidedPasswordHash
                ) THEN 1 ELSE 0 END AS AuthenticationResult;
        ";

            // Create a SqlConnection and a SqlCommand
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sqlQuery, connection))
            {
                // Add parameters to the SqlCommand
                command.Parameters.AddWithValue("@Email", providedEmail);
                command.Parameters.AddWithValue("@PasswordHash", providedPasswordHash);

                //try
                //{
                    // Open the connection
                    connection.Open();

                    // Execute the query and retrieve the result
                    int authenticationResult = (int)command.ExecuteScalar();
                    if (authenticationResult != 1)
                    {
                        throw new Exception("wrong password");
                    }
                    //TEMP LOGIN END

                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                      _config["Jwt:Issuer"],
                      null,
                      expires: DateTime.Now.AddMinutes(120),
                      signingCredentials: credentials);

                    var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                    return Ok(token);
              //  }
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error: " + ex.Message);
                //}

                return BadRequest("Invalid username or password");
            }
        }
    }

}
