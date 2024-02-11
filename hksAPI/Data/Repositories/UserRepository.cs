using hksAPI.Models;
using Microsoft.Data.SqlClient;

namespace hksAPI.Data.Repositories
{
    public class UserRepository : ICrudGeneric<User>
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }
        public string CheckEntity(User entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM [User] WHERE idUser = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }
        public User GetById(int id)
        {
            int roleId = 0; // Initializing to 0 as default value

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT u.idUser, u.username, u.passwordHash, r.idRoles FROM [User] u INNER JOIN roles r ON r.idRoles = u.roleId WHERE u.idUser = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    roleId = Convert.ToInt32(reader["idRoles"]);
                }

                reader.Close();
            }

            // Creating a new User object with only the RoleId property set
            User user = new User { RoleId = roleId };
            return user;
        }

        public User GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO [User] (username, passwordHash, roleId) VALUES (@Username, @PasswordHash, @RoleId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@RoleId", user.RoleId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Update(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE [User] SET username = @Username, passwordHash = @PasswordHash, roleId = @RoleId WHERE idUser = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@RoleId", user.RoleId);
                command.Parameters.AddWithValue("@UserId", user.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
