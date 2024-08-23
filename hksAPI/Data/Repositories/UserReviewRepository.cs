using hksAPI.Models;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Utilities;
using System.Data;

namespace hksAPI.Data.Repositories
{
    public class UserReviewRepository : ICrudGeneric<UserReview>
    {

        private readonly string _connectionString;

        public UserReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }

        public string CheckEntity(UserReview entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM UserReview WHERE IdUserReview = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<UserReview> GetAll()
        {
            List<UserReview> reviews = new List<UserReview>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM UserReview";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    reviews.Add(new UserReview
                    {
                        IdUserReview = Convert.ToInt32(reader["IdUserReview"]),
                        Grade = Convert.ToInt32(reader["Grade"]),
                        Review = reader["Review"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BreederId = Convert.ToInt32(reader["BreederId"])
                    });
                }
            }

            return reviews;
        }

        public IEnumerable<UserReview> GetAllByParametar(string breederID)
        {
            int idBreeder = int.Parse(breederID);

            var reviews = new List<UserReview>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                select u.BreederId, u.Grade, u.IdUserReview, u.Review, u.UserId 
                from UserReview u 
                inner join Breeder b on b.IdBreeder = u.BreederId
                where b.IdBreeder = @idBreeder";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@idBreeder", SqlDbType.Int) { Value = idBreeder });

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var review = new UserReview
                            {
                                BreederId = reader.GetInt32(0),
                                Grade = int.Parse(reader.GetString(1)),
                                IdUserReview = reader.GetInt32(2),
                                Review = reader.GetString(3),
                                UserId = reader.GetInt32(4)
                            };
                            reviews.Add(review);
                        }
                    }
                }
            }

            return reviews;
        }
    

    public UserReview GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM UserReview WHERE IdUserReview = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new UserReview
                    {
                        IdUserReview = Convert.ToInt32(reader["IdUserReview"]),
                        Grade = Convert.ToInt32(reader["Grade"]),
                        Review = reader["Review"].ToString(),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        BreederId = Convert.ToInt32(reader["BreederId"])
                    };
                }
                return null;
            }
        }

        public UserReview GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(UserReview review)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO UserReview (Grade, Review, UserId, BreederId) 
                                 VALUES (@Grade, @Review, @UserId, @BreederId)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Grade", review.Grade);
                command.Parameters.AddWithValue("@Review", review.Review);
                command.Parameters.AddWithValue("@UserId", review.UserId);
                command.Parameters.AddWithValue("@BreederId", review.BreederId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(UserReview review)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE UserReview 
                                 SET Grade = @Grade, Review = @Review, UserId = @UserId, BreederId = @BreederId 
                                 WHERE IdUserReview = @Id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Grade", review.Grade);
                command.Parameters.AddWithValue("@Review", review.Review);
                command.Parameters.AddWithValue("@UserId", review.UserId);
                command.Parameters.AddWithValue("@BreederId", review.BreederId);
                command.Parameters.AddWithValue("@Id", review.IdUserReview);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
