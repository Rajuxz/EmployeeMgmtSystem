using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EmployeeMgmtSystem.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly EmployeeDbContext _dbContext;
        internal DbSet<T> database;

        public Repository(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
            database = _dbContext.Set<T>();
            //For example: _dbContext.EmployeeTable == database. 
        }

        public void Add(T entity)
        {
            database.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = database;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = database;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            database.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            database.RemoveRange(entities);
        }

        public T FindById(int id)
        {
            DbSet<T>? query = database;
            return query.Find(id);
        }

     
    }
}
