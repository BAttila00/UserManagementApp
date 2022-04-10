using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UserManagementApp.Services;

namespace UserManagementApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("/Account/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Account/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IFormCollection collection)
        {
            string userName = collection["UserName"];
            string password = collection["Password"];

            if (_userService.CredentialsCorrect(userName, password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    //new Claim(ClaimTypes.Role, "Administrator"),
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                   
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                Debug.WriteLine(User.FindFirstValue(ClaimTypes.Name));
                return RedirectToAction("Index", "User");
            }

            return RedirectToAction("Index");


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
