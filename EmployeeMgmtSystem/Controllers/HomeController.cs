using EmployeeMgmtSystem.Models;
using EmployeeMgmtSystem.Repository.IRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net;
using System.Security.Claims;
using System.Web;

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

		
		public IActionResult Login(string? ReturnUrl = null)
		{
			LoginViewModel loginVm = new LoginViewModel();
			ViewData["ReturnUrl"] = ReturnUrl;
            return View(loginVm);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM,string? ReturnUrl=null)
		{

            var admin = _adminRepo.Get(x=>x.Email == loginVM.Email.ToLower());
			if (admin != null)
			{
				if(admin.Password ==  loginVM.Password)
				{
					var claim = new List<Claim>(){
						new Claim(ClaimTypes.Email, loginVM.Email),
						new Claim(ClaimTypes.Name, admin.Name),
						new Claim(ClaimTypes.Role,"Admin" ),
					};
					var identity = new ClaimsIdentity(claim,CookieAuthenticationDefaults.AuthenticationScheme);
					var principal = new ClaimsPrincipal(identity);
					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
						principal,new AuthenticationProperties()
					{
						IsPersistent = true
					});

					TempData["SuccessMessage"] = $"Welcome {admin.Name}";
					return LocalRedirect(ReturnUrl);
				}
				else
				{
                    TempData["ErrorMessage"] = "Opps! Incorrect Password";
					return RedirectToAction("Login");
				}
			}
            TempData["ErrorMessage"] = "Opps! No Admin found.";
            return RedirectToAction("Login");
        }


		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index");
		}
	}
}
