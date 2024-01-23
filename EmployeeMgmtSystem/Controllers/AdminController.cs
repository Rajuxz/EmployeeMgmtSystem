using EmployeeMgmtSystem.Models.Admin;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;

namespace EmployeeMgmtSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepo;
        public AdminController(IAdminRepository adminRepo) { 
            _adminRepo = adminRepo;
        }
       public IActionResult EditAdmin(string emailAddress)
        {
            var admin = _adminRepo.Get(x => x.Email == emailAddress);
            if (admin != null)
            {
                var adminData = new AdminViewModel()
                {
                    Id = admin.Id,
                    Name = admin.Name,
                    Email = admin.Email,
                    ProfilePath = admin.Profile
                };
                return View(adminData);
            }
            TempData["ErrorMessage"] = "No Admin Found";
            return RedirectToAction("Index", "Employee");
        }

        [HttpPost]
        public IActionResult EditAdmin(AdminViewModel adminVm)
        {
            var admin = _adminRepo.Get(x => x.Id == adminVm.Id);
            if (admin != null)
            {
                if(adminVm.Profile != null)
                {
                    string filename = _adminRepo.SaveFileAndReturnName("Admin/images", adminVm.Profile);
                    admin.Profile = filename;
                }
                admin.Name = adminVm.Name;
                admin.Email = adminVm.Email;

                _adminRepo.Update(admin);
                _adminRepo.Save();

                TempData["SuccessMessage"] = "Data updated Successfully.";
                return RedirectToAction("Index", "Employee");
            }
            TempData["ErrorMessage"] = "No admin found.";
            return RedirectToAction("Index", "Employee");
            
        }
    }
}
