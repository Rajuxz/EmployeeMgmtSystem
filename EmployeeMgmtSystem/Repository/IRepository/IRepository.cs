using System.Collections.Generic;
using System.Linq.Expressions;

namespace EmployeeMgmtSystem.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        IEnumerable<T> GetAll();

        IEnumerable<T> GetAllData(params Expression<Func<T, object>>[] includes);
        T Get(Expression<Func<T, bool>> filter);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        public T FindById(int id);

        public int Count();
    }
}
