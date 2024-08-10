using hksAPI.Models;
using hksAPI.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace hksAPI.Data.Repositories
{
    public class LoginRepository : ICrudGeneric<LoginRequest> 
    {
        private readonly string _connectionString;
        private IConfiguration _config;

        public LoginRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
            _config = configuration;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LoginRequest> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LoginRequest> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public LoginRequest GetById(int id)
        {
            throw new NotImplementedException();
        }

        public LoginRequest GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(LoginRequest entity)
        {
            throw new NotImplementedException();
        }

        public void Update(LoginRequest entity)
        {
            throw new NotImplementedException();
        }

        public string CheckEntity(LoginRequest loginRequest)
        {
            string providedEmail = loginRequest.Username;

            string providedPasswordHash = loginRequest.Password;

            string sqlQuery = @"
            DECLARE @ProvidedEmail NVARCHAR(255) = @Email;
            DECLARE @ProvidedPasswordHash NVARCHAR(255) = @PasswordHash;

            SELECT
                CASE WHEN EXISTS (
                    SELECT 1
                    FROM [User]
                    WHERE username = @ProvidedEmail AND passwordHash = @ProvidedPasswordHash
                ) THEN 1 ELSE 0 END AS AuthenticationResult;";

            // Create a SqlConnection and a SqlCommand
            using (SqlConnection connection = new SqlConnection(_connectionString))
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

                return token.ToString();

            }
        }



           

    }
}