﻿using hksAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

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
            string result;

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand("checkUserCredentials", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", entity.Username);
                    command.Parameters.AddWithValue("@roleId", entity.RoleId);
                    command.Parameters.AddWithValue("@passwordHash", hksAPI.Services.PasswordHasher.HashPassword(entity.PasswordHash));                    
                    connection.Open();
                    result = command.ExecuteScalar().ToString();
                }
            }
            return result;
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
            string query = "SELECT * FROM [User] u WHERE u.username = @username AND u.roleId = 3";
            User user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(new SqlParameter("@username", SqlDbType.NVarChar, 50)).Value = name;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = (int)reader["idUser"],
                        
                            };
                        }
                    }
                }

            }
            return user;
        }

        public void Insert(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    IF NOT EXISTS (SELECT 1 FROM [User] WHERE username = @Username)
                    BEGIN
                        INSERT INTO [User] (username, passwordHash, roleId) VALUES (@Username, @PasswordHash, @RoleId);
                    END";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@PasswordHash", hksAPI.Services.PasswordHasher.HashPassword(user.PasswordHash));
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
                command.Parameters.AddWithValue("@PasswordHash", hksAPI.Services.PasswordHasher.HashPassword(user.PasswordHash));
                command.Parameters.AddWithValue("@RoleId", user.RoleId);
                command.Parameters.AddWithValue("@UserId", user.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
