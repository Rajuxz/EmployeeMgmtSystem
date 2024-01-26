using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IDepartmentRepository _departmentRepo;
        public EmployeeController(IEmployeeRepository employeeRepo,IDepartmentRepository departmentRepo, ILogger<EmployeeController> logger) {
            _employeeRepo = employeeRepo;
            _logger = logger;
            _departmentRepo = departmentRepo;
        }

        public IActionResult Index()
        {
            var res = _employeeRepo.GetAll().ToList();
            int data = res.Count();
            ViewBag.EmployeeCount = data;
            return View();
        }
        public IActionResult GetData()
        {
            List<EmployeeModel> employees = _employeeRepo.GetAllData(e=>e.Department).ToList();
            return Json(employees);
        }
        public IActionResult Employees()
        {

            return View();
        }
        public IActionResult AddEmployee()
        {
            List<Department> departments = _departmentRepo.GetAll().ToList();
            EmployeeViewModel employeeVm = new EmployeeViewModel()
            {
                Departments = departments
            };
            return View(employeeVm);
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeViewModel employeeVm)
        {
            try
            {
                string filename = "default.png";
                if (employeeVm.Image != null)
                { 
                   filename = _employeeRepo.SaveFileAndReturnName("images", employeeVm.Image);
                }
                //Check if email and phone is already in use or not.
               if(_employeeRepo.Get(x=>x.Email == employeeVm.Email || x.Phone == employeeVm.Phone) != null)
               {
                    TempData["ErrorMessage"] = "Email or phone is already in use";
                    //⚠️ If this block is executed, and you're returning view
                    // Department select list will be empty so it will throw exception.
                    //Always use Name of Action.
                    return RedirectToAction("AddEmployee");
               }

               //if not...
                else
                {
                    var employee = new EmployeeModel()
                    {
                        Name = employeeVm.Name,
                        Address = employeeVm.Address,
                        Salary = employeeVm.Salary,
                        Phone = employeeVm.Phone,
                        Position = employeeVm.Position,
                        Email = employeeVm.Email,
                        Image = filename,
                        DepartmentId = employeeVm.SelectedDepartmentId
                    };
                    _employeeRepo.Add(employee);
                    _employeeRepo.Save();
                    SetMessage("Data Inserted Successfully !", "SuccessMessage");
                    return RedirectToAction("Employees");
                }

            }catch(Exception e)
            {
                SetMessage($"Opps !! Cannot Add Data. {e.Message}", "ErrorMessage");
                return RedirectToAction("Employees");
            }   
        }
        public IActionResult UpdateEmployee(int id)
        {
            var employee = _employeeRepo.Get(x => x.Id == id);
            //var departments = _departmentRepo.GetAll().ToList();
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
                };
                return View(employeeData);
            }
            else
            {
                SetMessage("Opps !! Data Not Found. ", "ErrorMessage");
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
                    if (updateVM.Image != null)
                    {
                        string filename = _employeeRepo.SaveFileAndReturnName("images", updateVM.Image);
                        employee.Image = filename;
                    }
                        employee.Name = updateVM.Name;
                        employee.Address = updateVM.Address;
                        employee.Salary = updateVM.Salary;
                        employee.Phone = updateVM.Phone;
                        employee.Email = updateVM.Email;
                        _employeeRepo.Update(employee);
                        _employeeRepo.Save();

                    SetMessage("Data Successfully Updated.", "SuccessMessage");
                    return RedirectToAction("Employees");
                }
                    SetMessage("Inconsistant Modal State !", "ErrorMessage");
                    return Redirect("Employees");
            }catch(Exception ex) {
                SetMessage($"Something went wrong. {ex.ToString()}","ErrorMessage");
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
                SetMessage("Data Successfully Deleted.", "SuccessMessage");
                return RedirectToAction("Employees");
            }catch(Exception e)
            {
                SetMessage($"Cannot Delete data. {e.Message}", "ErrorMessage");
                return RedirectToAction("Employees");
            }
        }

        public IActionResult ViewEmployee(int id)
        {
            var employee = _employeeRepo.Get(emp => emp.Id == id);
            return View(employee);
        }

        public IActionResult EmployeeSheet(int id)
        {
            try
            {
                var employees = _employeeRepo.FindById(id);
                return View(employees);
                                
            }catch(Exception ex)
            {
                TempData["WarningMessage"] = $"There is a problem, Please fix it first : {ex.Message}";
                return RedirectToAction("Employees");
            }
        }

        public void SetMessage(string message, string messageType)
        {
            TempData[messageType] = message;
        }
    }
}
