using hksAPI.Models;
using Microsoft.Data.SqlClient;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;


namespace hksAPI.Data.Repositories
{
    public class DogRepository : ICrudGeneric<Dog>
    {
        private readonly string _connectionString;

        public DogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }

        public string CheckEntity(Dog entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Dog WHERE IdDog = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Dog> GetAll()
        {
            List<Dog> dogs = new List<Dog>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Dog";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    dogs.Add(new Dog
                    {
                        IdDog = Convert.ToInt32(reader["IdDog"]),
                        BreedName = reader["BreedName"].ToString(),
                        AvgWeightFemale = Convert.ToInt32(reader["AvgWeightFemale"]),
                        AvgWeightMale = Convert.ToInt32(reader["AvgWeightMale"]),
                        Description = reader["Description"].ToString(),
                        Image = reader["Image"].ToString()
                    });
                }
            }

            return dogs;
        }

        public IEnumerable<Dog> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public Dog GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Dog WHERE IdDog = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Dog
                    {
                        IdDog = Convert.ToInt32(reader["IdDog"]),
                        BreedName = reader["BreedName"].ToString(),
                        AvgWeightFemale = Convert.ToInt32(reader["AvgWeightFemale"]),
                        AvgWeightMale = Convert.ToInt32(reader["AvgWeightMale"]),
                        Description = reader["Description"].ToString(),
                        Image = reader["Image"].ToString()
                    };
                }
                return null;
            }
        }

        public Dog GetByName(string breedName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Dog WHERE UPPER(TRIM(BreedName)) = UPPER(TRIM(@BreedName))";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BreedName", breedName.Trim().ToUpper());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Dog
                    {
                        IdDog = Convert.ToInt32(reader["IdDog"]),
                        BreedName = reader["BreedName"].ToString(),
                        AvgWeightFemale = Convert.ToInt32(reader["AvgWeightFemale"]),
                        AvgWeightMale = Convert.ToInt32(reader["AvgWeightMale"]),
                        Description = reader["Description"].ToString(),
                        Image = reader["Image"].ToString()
                    };
                }
                return null;
            }
        }

        public void Insert(Dog dog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Dog (BreedName, AvgWeightFemale, AvgWeightMale, Description, Image) 
                                 VALUES (@BreedName, @AvgWeightFemale, @AvgWeightMale, @Description, @Image)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BreedName", dog.BreedName.Trim().ToUpper());
                command.Parameters.AddWithValue("@AvgWeightFemale", dog.AvgWeightFemale);
                command.Parameters.AddWithValue("@AvgWeightMale", dog.AvgWeightMale);
                command.Parameters.AddWithValue("@Description", dog.Description);
                command.Parameters.AddWithValue("@Image", dog.Image);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(Dog dog)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Dog
                                 SET BreedName = @BreedName, AvgWeightFemale = @AvgWeightFemale, AvgWeightMale = @AvgWeightMale,
                                     Description = @Description, Image = @Image 
                                 WHERE IdDog = @IdDog";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BreedName", dog.BreedName);
                command.Parameters.AddWithValue("@AvgWeightFemale", dog.AvgWeightFemale);
                command.Parameters.AddWithValue("@AvgWeightMale", dog.AvgWeightMale);
                command.Parameters.AddWithValue("@Description", dog.Description);
                command.Parameters.AddWithValue("@Image", dog.Image);
                command.Parameters.AddWithValue("@IdDog", dog.IdDog);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
