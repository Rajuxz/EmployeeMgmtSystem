﻿using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeeMgmtSystem.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        IWebHostEnvironment _webHostEnv;        
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IAdminRepository _adminRepository;
        public EmployeeController(IEmployeeRepository employeeRepo,IWebHostEnvironment webHostEnv,IAdminRepository adminRepository) {
            _employeeRepo = employeeRepo;
            _webHostEnv = webHostEnv;
            _adminRepository = adminRepository;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetData()
        {
            List<EmployeeModel> employees = _employeeRepo.GetAll().ToList();
            return Json(employees);
        }
        public IActionResult Employees()
        {
            try
            {
                return View();
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
                   filename = _employeeRepo.SaveFileAndReturnName("images", employeeVm.Image);
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
                SetMessage("Data Inserted Successfully !", "SuccessMessage");
                return RedirectToAction("Employees");

            }catch(Exception e)
            {
                SetMessage($"Opps !! Cannot Add Data. {e.Message}", "ErrorMessage");
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
                        employee.Department = updateVM.Department;
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

        public void SetMessage(string message, string messageType)
        {
            TempData[messageType] = message;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl)
        {

            var admin = _adminRepository.Get(x => x.Email == email && x.Password == password);
            if(admin!= null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,admin.Name)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "Employees");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
                return Redirect(returnUrl == null ? "/Employee/Employees/" : "/Employee/Login");
            }
            else
            {
                return View();
            }

        }

		[AllowAnonymous]
        [HttpPost]
		public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
