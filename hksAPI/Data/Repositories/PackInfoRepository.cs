using hksAPI.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace hksAPI.Data.Repositories
{


    public class PackInfoRepository : ICrudGeneric<PackInfo>
    {
        private readonly string _connectionString;

        public PackInfoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }
        public string CheckEntity(PackInfo entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PackInfo> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PackInfo> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public PackInfo GetById(int id)
        {
            throw new NotImplementedException();
        }

        public PackInfo GetByName(string name)
        {
            string storedProcedure = "GetPackInfo";
            PackInfo packInfo = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@seller_contact_info", name);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            packInfo = new PackInfo
                            {
                                Dog = reader["BreedName"].ToString(),
                                BirthDate = Convert.ToDateTime(reader["BirtDate"]),
                                MaleCount = Convert.ToInt32(reader["Male"]),
                                FMaleCount = Convert.ToInt32(reader["FMale"]),
                                Description = reader["PackDescription"].ToString()
                            };
                            packInfo.SellerContactInfo = name;
                        }
                    }
                }
            }

            return packInfo;
        }


        public void Insert(PackInfo entity)
        {
            throw new NotImplementedException();
        }


        public void Update(PackInfo entity)
        {
            string storedProcedure = "PostPack";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@seller_contact_info", entity.SellerContactInfo);
                    command.Parameters.AddWithValue("@description", entity.Description);
                    command.Parameters.AddWithValue("@price", entity.Price);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
