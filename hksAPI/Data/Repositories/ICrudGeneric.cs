using System.Collections.Generic;

namespace hksAPI.Data.Repositories
{
    public interface ICrudGeneric<T>
    {
        T GetById(int id);
        T GetByName(string name);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllByParametar(string parametar);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
