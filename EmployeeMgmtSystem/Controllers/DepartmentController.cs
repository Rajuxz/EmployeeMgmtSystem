using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmtSystem.Controllers
{
    [Authorize(Roles ="Admin")]
    public class DepartmentController : Controller
    {
       public IActionResult AddDepartment()
        {
            return View();
        }
    }
}
