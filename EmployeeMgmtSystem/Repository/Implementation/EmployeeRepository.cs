using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmtSystem.Repository.Implementation
{
    public class EmployeeRepository : Repository<EmployeeModel>, IEmployeeRepository
    {
        private IWebHostEnvironment _environment;
        private EmployeeDbContext _employeeDbContext;
        public EmployeeRepository(EmployeeDbContext employeeDbContext, IWebHostEnvironment environment) : base(employeeDbContext)
        {
            _employeeDbContext = employeeDbContext;
            _environment = environment;
        }

        public void Save()
        {
            _employeeDbContext.SaveChanges();
        }

        public string? SaveFileAndReturnName(string path, IFormFile file)
        {
            try
            {
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

        public void Update(EmployeeModel model)
        {
            _employeeDbContext.Employees?.Update(model);
        }

    }
}