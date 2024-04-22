using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementSystem.MVC.Models;
using QueueManagementSystem.MVC.Services;

namespace Queue_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel user, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var serviceProvider = await _authService.Authenticate(user);
            

            if (serviceProvider == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View();
            }

            string? role = serviceProvider.IsAdmin ? "Admin" : "";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, serviceProvider.Id.ToString()),
                new Claim(ClaimTypes.Name, serviceProvider.FullNames),
                new Claim(ClaimTypes.Email, serviceProvider.Email),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieScheme");

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync("MyCookieScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

            return LocalRedirect(returnUrl);       
        }

        //TODO: logout

    }
}