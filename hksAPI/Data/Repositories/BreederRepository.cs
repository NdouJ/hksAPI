using hksAPI.Models;
using Microsoft.Data.SqlClient;

namespace hksAPI.Data.Repositories
{
    public class BreederRepository : ICrudGeneric<Breeder>
    {

        private readonly string _connectionString;

        public BreederRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }

        public string CheckEntity(Breeder entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            DELETE FROM Breeder
            WHERE IdBreeder = @IdBreeder ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdBreeder", id);

                    command.ExecuteNonQuery();
                }
            }
        }


        public IEnumerable<Breeder> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Breeder> GetAllByParametar(string parametar)
        {
            List<Breeder> breeders = new List<Breeder>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            SELECT b.BreederName, b.BreederContact, b.OIB, b.IdBreeder
            FROM Breeder b
            INNER JOIN pack p ON b.IdBreeder = p.BreederID
            LEFT JOIN Dog d ON d.idDog = p.BreedNameID
            WHERE UPPER(TRIM(d.BreedName)) = UPPER(TRIM(@BreedName))";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BreedName", parametar);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Breeder breeder = new Breeder
                            {
                                BreederName = reader["BreederName"].ToString(),
                                BreederContact = reader["BreederContact"].ToString(),
                                OIB = reader["OIB"].ToString(),
                                IdBreeder = Int32.Parse(reader["IdBreeder"].ToString())
                            };

                            breeders.Add(breeder);
                        }
                    }
                }
            }

            return breeders;
        }


        public Breeder GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Breeder GetByName(string name)
        {
            Breeder  breeder = new();    
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT * FROM Breeder b WHERE UPPER(b.BreederName) LIKE UPPER(@SearchName)", connection))
                {
                    command.Parameters.AddWithValue("@SearchName", "%" + name + "%");

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            breeder.BreederName = reader["BreederName"].ToString();
                            breeder.BreederContact = reader["BreederContact"].ToString();
                            breeder.OIB = reader["OIB"].ToString();
                            breeder.IdBreeder = Int32.Parse(reader["IdBreeder"].ToString());
                        }
                    }
                }
            }

            return breeder;
        }

        public void Insert(Breeder entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            INSERT INTO Breeder (BreederName, BreederContact, OIB)
            VALUES (@BreederName, @BreederContact, @OIB);
            SELECT SCOPE_IDENTITY();
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BreederName", entity.BreederName);
                    command.Parameters.AddWithValue("@BreederContact", entity.BreederContact);
                    command.Parameters.AddWithValue("@OIB", entity.OIB);

                    // ExecuteScalar is used to retrieve the newly inserted identity (auto-increment) value
                    int newId = Convert.ToInt32(command.ExecuteScalar());

                    // Assign the new ID to the entity
                    entity.IdBreeder = newId;
                }
            }
        }


        public void Update(Breeder entity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
            UPDATE Breeder
            SET BreederName = @BreederName,
                BreederContact = @BreederContact,
                OIB = @OIB
            WHERE IdBreeder = @IdBreeder
        ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BreederName", entity.BreederName);
                    command.Parameters.AddWithValue("@BreederContact", entity.BreederContact);
                    command.Parameters.AddWithValue("@OIB", entity.OIB);
                    command.Parameters.AddWithValue("@IdBreeder", entity.IdBreeder);

                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
