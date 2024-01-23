using EmployeeMgmtSystem.Models.Domain;

namespace EmployeeMgmtSystem.Repository.IRepository
{
	public interface IAdminRepository:IRepository<AdminModel>
	{
        public string? SaveFileAndReturnName(string path, IFormFile formFile);
        public void Update(AdminModel admin);
        public void Save();
    }
}
