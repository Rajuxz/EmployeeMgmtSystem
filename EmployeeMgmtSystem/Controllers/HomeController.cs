using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmtSystem.Controllers
{
	public class HomeController : Controller
	{

		private readonly IAdminRepository _adminRepo;
		public HomeController(IAdminRepository adminRepo)
		{
			_adminRepo = adminRepo;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(string email, string password,string ReturnUrl)
		{
			var admin = _adminRepo.Get(x => x.Email == email);
			if(admin!=null)
			{
				if(admin.Password == password)
				{
					return Content("Password milo");
				}
				else
				{
					return Content("Password Milena.");
				}
			}
			return Content("Admin null chha ");

		}


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index");
		}
	}
}
