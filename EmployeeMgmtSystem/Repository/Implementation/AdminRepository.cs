using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;

namespace EmployeeMgmtSystem.Repository.Implementation
{
	public class AdminRepository : Repository<AdminModel>, IAdminRepository
	{
        private IWebHostEnvironment _environment;
        private EmployeeDbContext _employeeDbContext;
        public AdminRepository(EmployeeDbContext dbContext, IWebHostEnvironment environment) : base(dbContext)
        {
            _employeeDbContext = dbContext;
            _environment = environment;
        }
        public string? SaveFileAndReturnName(string path, IFormFile file)
           {
              try{
                 if (file != null)
                 {
                      string uniqueName = Guid.NewGuid().ToString() + file.FileName;
                      string serverLocation = Path.Combine(_environment.WebRootPath, Path.Combine(path, uniqueName));
                      file.CopyTo(new FileStream(serverLocation, FileMode.Create));
                      return uniqueName;
                 }
                 else
                 {
                      return "default.png";
                 }
              }
              catch (Exception ex)
              {
                  Console.WriteLine($"Exception saving file: {ex.Message}");
                  return "default.png";
              }
            }
        public void Save()
        {
            _employeeDbContext.SaveChanges();
        }

        public void Update(AdminModel admin)
        {
            _employeeDbContext.Update(admin);
        }
    }
}
