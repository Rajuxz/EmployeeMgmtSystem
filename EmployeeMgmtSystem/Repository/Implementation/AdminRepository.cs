using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;

namespace EmployeeMgmtSystem.Repository.Implementation
{
	public class AdminRepository : Repository<AdminModel>, IAdminRepository
	{
		public AdminRepository(EmployeeDbContext dbContext) : base(dbContext)
		{
		}
	}
}
