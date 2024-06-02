using hksAPI.Models;
using Microsoft.Data.SqlClient;


namespace hksAPI.Data.Repositories
{
    public class SellerRepository : ICrudGeneric<Seller>
    {
        private readonly string _connectionString;

        public SellerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }

        public string CheckEntity(Seller entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Seller WHERE idSeller = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Seller> GetAll()
        {
            List<Seller> sellers = new List<Seller>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Seller";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    sellers.Add(new Seller
                    {
                        IdSeller = Convert.ToInt32(reader["idSeller"]),
                        BreederName = reader["breeder_name"].ToString(),
                        ContactInfo = reader["contact_info"].ToString(),
                        OIB = reader["OIB"].ToString()
                    });
                }
                reader.Close();
            }
            return sellers;
        }

        public IEnumerable<Seller> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public Seller GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Seller WHERE idSeller = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Seller
                    {
                        IdSeller = Convert.ToInt32(reader["idSeller"]),
                        BreederName = reader["breeder_name"].ToString(),
                        ContactInfo = reader["contact_info"].ToString(),
                        OIB = reader["OIB"].ToString()
                    };
                }
                return null;
            }
        }

        public Seller GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(Seller entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Seller (breeder_name, contact_info, OIB) VALUES (@BreederName, @ContactInfo, @OIB)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BreederName", entity.BreederName);
                command.Parameters.AddWithValue("@ContactInfo", entity.ContactInfo);
                command.Parameters.AddWithValue("@OIB", entity.OIB);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Seller entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Seller SET breeder_name = @BreederName, contact_info = @ContactInfo, OIB = @OIB WHERE idSeller = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BreederName", entity.BreederName);
                command.Parameters.AddWithValue("@ContactInfo", entity.ContactInfo);
                command.Parameters.AddWithValue("@OIB", entity.OIB);
                command.Parameters.AddWithValue("@Id", entity.IdSeller);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
