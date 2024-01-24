using EmployeeMgmtSystem.Models.Domain;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmtSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {

        private readonly IDepartmentRepository _departmentRepo;
        public DepartmentController(IDepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;            
        }
        public IActionResult AddDepartment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDepartment([FromBody] Department department)
        {
            if(department != null)
            {
                var departmentObj = new Department()
                {
                    Name = department.Name,
                };
                _departmentRepo.Add(departmentObj);
                _departmentRepo.Save();

                var json = new
                {
                    message = "Succeed"
                };
                 return Json(json);
            }
            else
            {
                return Json("this");
                
            }
        }
        public IActionResult GetDepartments()
        {
            try
            {
                var department = _departmentRepo.GetAll().ToList();
                return Json(department);
            }
            catch (Exception e)
            {
                var data = new
                {
                    message = e.Message 
                };
                return Json(data);
            }
        }
    }
}
