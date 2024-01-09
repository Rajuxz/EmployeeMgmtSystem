using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
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
        public readonly EmployeeDbContext _employeeDbContext;
        public EmployeeController(EmployeeDbContext employeeDbContext,IWebHostEnvironment webHostEnv) {
            _employeeDbContext = employeeDbContext;
            _webHostEnv = webHostEnv;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Employees(int pg = 1,string searchQuery = "",int sortType = 1) {
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }           
            if (String.IsNullOrEmpty(searchQuery) || sortType == 0)
            { 
                var result = Paginate(5,pg,searchQuery="");
                return View(result);
            }
            else
            {
                var searchResult = Paginate(5, pg, searchQuery);
                return View(searchResult);
            }
        }
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel employeeVm)
        {
            try
            {
                string filename = "";
                if(employeeVm.Image !=  null)
                {
                    string uploadFolder = Path.Combine(_webHostEnv.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString()+employeeVm.Image.FileName;
                    string filePath = Path.Combine(uploadFolder,filename);
                    employeeVm.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                if(_employeeDbContext.Employees.Any(e=>e.Email == employeeVm.Email || e.Phone == employeeVm.Phone)) {
                    TempData["ErrorMessage"] = "Employee with provided email or phone already exists";
                    return RedirectToAction("Employees");
                }
                else
                {
                    var employee = new EmployeeModel()
                    {
                        Name = employeeVm.Name,
                        Address = employeeVm.Address,
                        Salary  = employeeVm.Salary,
                        Phone   = employeeVm.Phone,
                        Position    = employeeVm.Position,
                        Email = employeeVm.Email,
                        Department = employeeVm.Department,
                        Image = filename
                    };
                _employeeDbContext.Employees?.Add(employee);
                _employeeDbContext.SaveChanges();
                TempData["SuccessMessage"] = "Employee added successfully.";
                return RedirectToAction("Employees");
                }
            }catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Cannot add Employee :{e.Message}";
                return RedirectToAction("Employees");
            }
        }
        public IActionResult UpdateEmployee(int Id)
        {
            var employee = _employeeDbContext.Employees?.FirstOrDefault(x => x.Id == Id);
            if(employee != null)
            {
                var employeeData = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Address = employee.Address,
                    Salary = employee.Salary,
                    Phone = employee.Phone,
                    Position = employee.Position,
                    Email = employee.Email,
                    Department = employee.Department
                };
                return View(employeeData);
            }
            else
            {

                TempData["ErrorMessage"] = "Couldnot find data with given Id";
                return RedirectToAction("Employees");
            }
        }
        [HttpPost]
        public IActionResult UpdateEmployee(UpdateEmployeeViewModel updateEmployeeVM)
        {
            var employee = _employeeDbContext.Employees?.Find(updateEmployeeVM.Id);
            if(employee != null)
            {
                //check if email or phone is already exists or not.
               
                    employee.Name = updateEmployeeVM.Name;
                    employee.Address = updateEmployeeVM.Address;
                    employee.Salary = updateEmployeeVM.Salary;
                    employee.Phone = updateEmployeeVM.Phone;
                    employee.Position = updateEmployeeVM.Position;
                    employee.Email = updateEmployeeVM.Email;
                    employee.Department = updateEmployeeVM.Department;
                    _employeeDbContext.SaveChanges();
                    TempData["SuccessMessage"] = "Data Updated Successfully.";
                    return RedirectToAction("Employees");   
                
            }
            else
            {
                TempData["ErrorMessage"] = "Maybe Id in form is modified.";
                return RedirectToAction("Employees");
            }
        }
        public IActionResult DeleteEmployee(int Id)
        {
            var employee = _employeeDbContext.Employees?.Find(Id);
            if(employee != null)
            {
                _employeeDbContext.Employees?.Remove(employee);
                _employeeDbContext.SaveChanges();
                 TempData["SuccessMessage"] = "Data deleted Successfully !";
                 return RedirectToAction("Employees");
            }
            else
            {
                TempData["ErrorMessage"] = "Employee not found !";
                return RedirectToAction("Employees");
            }
        }

        public IActionResult ViewEmployee(int Id)
        {
            var employee = _employeeDbContext.Employees?.Find(Id);
            return View(employee);
        }

        public List<EmployeeModel> Paginate(int pagesize, int pg = 1,string searchquery = "")
        {
            string searchQuery = searchquery;
            int pageSize = 5;
            List<EmployeeModel> employeeData;
            if (pg < 1)
                pg = 1;

            if (pagesize > 5)
                pageSize = pagesize;
            if (!String.IsNullOrEmpty(searchQuery))
            {
                employeeData = _employeeDbContext.Employees.Where(e =>
                           e.Name.Contains(searchQuery) ||
                           e.Email.Contains(searchQuery) ||
                           e.Department.Contains(searchQuery)
                           ).ToList();
            }
            else
            {
             employeeData = _employeeDbContext.Employees?.ToList();
            }

            int resultCount = employeeData.Count();
            var pager = new Pager(resultCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = employeeData.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.pager = pager;
            return data;
        }
    }
}
