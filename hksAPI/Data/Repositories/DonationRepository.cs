using hksAPI.Models;
using Microsoft.Data.SqlClient;

namespace hksAPI.Data.Repositories
{
    public class DonationRepository : ICrudGeneric<Donation>
    {
        private readonly string _connectionString;
        public DonationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");

        }

        public string CheckEntity(Donation entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Donation> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Donation> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public Donation GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Donation GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(Donation entity)
        {
            string query = "INSERT INTO Donation (Amount, UserId) VALUES (@Amount, @UserId)";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Amount", entity.Amount);
                    command.Parameters.AddWithValue("@UserId", entity.UserId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Donation entity)
        {
            throw new NotImplementedException();
        }
    }
}
