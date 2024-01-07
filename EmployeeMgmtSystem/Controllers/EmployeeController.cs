using EmployeeMgmtSystem.DataContext;
using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;
using System.Xml.Linq;

namespace EmployeeMgmtSystem.Controllers
{
    public class EmployeeController : Controller
    {
        
        public readonly EmployeeDbContext _employeeDbContext;
        public EmployeeController(EmployeeDbContext employeeDbContext) {
                _employeeDbContext = employeeDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Employees(int pg = 1) {
            List<EmployeeModel> employeeData = _employeeDbContext.Employees?.OrderBy(e=>e.Name).ToList();
            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }

            int resCount = employeeData.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int resSkip = (pg - 1) * pageSize;
            var data = employeeData.Skip(resSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }
        public IActionResult AddEmployee(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel employeeVm)
        {
            try
            {
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

        public IActionResult ViewEmployee(int id)
        {
            return View();
        }
    }
}
