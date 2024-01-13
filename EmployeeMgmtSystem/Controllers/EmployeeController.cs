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
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel employeeVm)
        {
            try
            {
                string filename = "default.png";
                if (employeeVm.Image != null)
                { 
                    string uploadFolder = Path.Combine(_webHostEnv.WebRootPath, "images");
                    filename = Guid.NewGuid().ToString() + employeeVm.Image.FileName;
                    string filePath = Path.Combine(uploadFolder, filename);
                    employeeVm.Image.CopyTo(new FileStream(filePath, FileMode.Create));

                }
                var employee = new EmployeeModel()
                {
                    Name = employeeVm.Name,
                    Address = employeeVm.Address,
                    Salary = employeeVm.Salary,
                    Phone = employeeVm.Phone,
                    Position = employeeVm.Position,
                    Email = employeeVm.Email,
                    Department = employeeVm.Department,
                    Image = filename
                };
                _employeeRepo.Add(employee);
                _employeeRepo.Save();
                TempData["SuccessMessage"] = "Employee Data Added Successfully.";
                return RedirectToAction("Employees");

            }catch(Exception e)
            {
                TempData["ErrorMessage"] = $"Cannot add data. {e.ToString()}";
                return RedirectToAction("Employees");
            }
            
        }
        public IActionResult UpdateEmployee(int id)
        {
            var employee = _employeeRepo.Get(x => x.Id == id);
            if (employee != null)
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
                    Department = employee.Department,
                };
                return View(employeeData);
            }
            else
            {
                TempData["ErrorMessage"] = "Opps !! Data Not Found. ";
                return RedirectToAction("Employees");
            }
        }
        [HttpPost]
        public IActionResult UpdateEmployee(UpdateEmployeeViewModel updateVM)
        {
            try
            {
                var employee = _employeeRepo.FindById(updateVM.Id);

                if (ModelState.IsValid)
                {
                    if(updateVM.Image != null)
                    {
                        string uploadFolder = Path.Combine(_webHostEnv.WebRootPath, "images");
                        string filename = Guid.NewGuid().ToString() + updateVM.Image.FileName;
                        string filePath = Path.Combine(uploadFolder, filename);
                        updateVM.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                        employee.Image = filename;
                    }

                    employee.Name = updateVM.Name;
                    employee.Address = updateVM.Address;
                    employee.Salary = updateVM.Salary;
                    employee.Phone = updateVM.Phone;
                    employee.Department = updateVM.Department;
                    employee.Email = updateVM.Email;
                    _employeeRepo.Update(employee);
                    _employeeRepo.Save();
                    TempData["SuccessMessage"] = "Data Updated Successfully!";
                    return RedirectToAction("Employees");
                }
                TempData["ErrorMessage"] = "Inconsistant Modal State !";
                return Redirect("Employees");
            }catch(Exception ex) {
                TempData["ErrorMessage"] = $"Something went wrong.{ex.ToString()}";
                return RedirectToAction("Employees");
            }
        }
        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                var employee = _employeeRepo.FindById(id);
                _employeeRepo.Remove(employee);
                _employeeRepo.Save();
                TempData["SuccessMessage"] = "Data Updated Successfully!";
                return RedirectToAction("Employees");
            }catch(Exception e)
            {
                TempData["ErrorMessage"] = $"Cannot Delete Data!{e.Message}";
                return RedirectToAction("Employees");
            }
        }

        public IActionResult ViewEmployee(int id)
        {
            var employee = _employeeRepo.Get(emp => emp.Id == id);
            return View(employee);
        }
    }
}
