namespace hksAPI.Data.Repositories
{
    public class BreederRepository<T> : ICrudGeneric<T>
    {

        private readonly string _connectionString;

        public BreederRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Local");
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAllByParametar(string parametar)
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public T GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public void Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
