using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;

namespace EmployeeMgmtSystem.Repository.Implementation
{
    public class EmployeeRepository : Repository<EmployeeModel>, IEmployeeRepository
    {
        private EmployeeDbContext _employeeDbContext;
        public EmployeeRepository(EmployeeDbContext employeeDbContext) : base(employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }
    
        public void Save()
        {
            _employeeDbContext.SaveChanges();
        }

        public void Update(EmployeeModel model)
        {
            _employeeDbContext.Employees?.Update(model);
        }
    }
}
