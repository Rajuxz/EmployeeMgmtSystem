using EmployeeMgmtSystem.DataContext;

namespace EmployeeMgmtSystem.Repository
{
    public class EmployeeRepository:IEmployee
    {
        private readonly EmployeeDbContext _dbContext;
        public EmployeeRepository( EmployeeDbContext dbContext) {
            _dbContext = dbContext;
        }
       
    }
}
