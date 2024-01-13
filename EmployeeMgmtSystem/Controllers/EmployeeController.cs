using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.Internal.TypeHandlers;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace EmployeeMgmtSystem.Controllers
{
    public class EmployeeController : Controller
    {
        IWebHostEnvironment _webHostEnv;        
        private readonly IEmployeeRepository _employeeRepo;
        public EmployeeController(IEmployeeRepository employeeRepo,IWebHostEnvironment webHostEnv) {
            _employeeRepo = employeeRepo;
            _webHostEnv = webHostEnv;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Employees()
        {
            try
            {
                List<EmployeeModel> employee = _employeeRepo.GetAll().ToList();
                return View(employee);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Aayo {ex.ToString()}");
                return View(ex);
            }
        }
    }
}
