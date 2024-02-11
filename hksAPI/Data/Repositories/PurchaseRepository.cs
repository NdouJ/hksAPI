using hksAPI.Models;
using Microsoft.Data.SqlClient;
using SixLabors.ImageSharp;
using System.Data;

namespace hksAPI.Data.Repositories
{
    public class PurchaseRepository : ICrudGeneric<Purchase>
    {
        private readonly string _connectionString;

        public PurchaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");

        }

        public string CheckEntity(Purchase entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> GetAll()
        {
            List<Purchase> basketItems = new();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT bi.Price, bi.Quantity, d.BreedName, br.BreederName 
                FROM BasketItem bi 
                INNER JOIN Basket ba ON bi.BasketID = ba.BasketID
                INNER JOIN [User] u ON u.idUser = ba.UserID
                INNER JOIN Pack p ON p.idPack = bi.PackID
                INNER JOIN Dog d ON d.idDog = p.BreedNameID
                INNER JOIN Breeder br ON br.IdBreeder = p.BreederID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchase item = new();
                                if (!Convert.IsDBNull(reader["Price"]))
                                {
                                    item.Price = Convert.ToDecimal(reader["Price"]);
                                }
                                else
                                {
                                    item.Price = 0;
                                }
                                item.Quantity = Convert.ToInt32(reader["Quantity"]);
                                item.BreedName = reader["BreedName"].ToString();
                                item.BreederName = reader["BreederName"].ToString();
                                basketItems.Add(item);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {

                    }
                }
            }

            return basketItems;
        }


        public IEnumerable<Purchase> GetAllByParametar(string userId)
        {
            int userIdInt = 0;
            try
            {
                userIdInt= Convert.ToInt32(userId);
            }
            catch (Exception)
            {

            }

            List<Purchase> basketItems = new();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                SELECT bi.Price, bi.Quantity, d.BreedName, br.BreederName 
                FROM BasketItem bi 
                INNER JOIN Basket ba ON bi.BasketID = ba.BasketID
                INNER JOIN [User] u ON u.idUser = ba.UserID
                INNER JOIN Pack p ON p.idPack = bi.PackID
                INNER JOIN Dog d ON d.idDog = p.BreedNameID
                INNER JOIN Breeder br ON br.IdBreeder = p.BreederID
                WHERE u.idUser = @UserID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = userIdInt;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Purchase item = new ();
                                if (!Convert.IsDBNull(reader["Price"]))
                                {
                                    item.Price = Convert.ToDecimal(reader["Price"]);
                                }
                                else
                                {
                                    item.Price = 0;
                                }
                                item.Quantity = Convert.ToInt32(reader["Quantity"]);
                                item.BreedName = reader["BreedName"].ToString();
                                item.BreederName = reader["BreederName"].ToString();
                                basketItems.Add(item);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {
                    }
                }
            }

            return basketItems;

        }

        public Purchase GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Purchase GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(Purchase purchase)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("MakePurchase", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = purchase.UserID;
                    command.Parameters.Add("@IdPack", SqlDbType.Int).Value = purchase.IdPack;
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = purchase.Quantity;
                    command.Parameters.Add("@SellerID", SqlDbType.Int).Value = purchase.SellerID;

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {

                    }
                }
            }
        }

        public void Update(Purchase entity)
        {
            throw new NotImplementedException();
        }
    }
}
