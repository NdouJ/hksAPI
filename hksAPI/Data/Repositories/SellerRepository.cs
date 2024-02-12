using hksAPI.Models;

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
            throw new NotImplementedException();
        }

        public IEnumerable<Seller> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Seller> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public Seller GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Seller GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(Seller entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Seller entity)
        {
            throw new NotImplementedException();
        }
    }
}
