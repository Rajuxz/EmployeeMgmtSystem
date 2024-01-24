using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;

namespace EmployeeMgmtSystem.Repository.Implementation
{
    public class DepartmentRepository:Repository<Department>, IDepartmentRepository
    {
        private readonly EmployeeDbContext _employeeDbContext;
        public DepartmentRepository(EmployeeDbContext employeeDbContext):base(employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
        }

       public void Update(Department model)
        {
            _employeeDbContext.Update(model);
        }
       public void Save()
        {
            _employeeDbContext.SaveChanges();
        }
    }
}
