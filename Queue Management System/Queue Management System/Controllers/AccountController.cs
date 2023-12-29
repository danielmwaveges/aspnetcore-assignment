using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Queue_Management_System.Models;
using Queue_Management_System.Services;

namespace Queue_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ServiceProviderService serviceproviderservice;
        public AccountController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("qmsdb");
            serviceproviderservice = new ServiceProviderService(connectionString);

        }
        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            //validate login credentials here
            Models.ServiceProvider serviceProvider = new Models.ServiceProvider();
            serviceProvider = await serviceproviderservice.getServiceProviderbyEmail(email);
            string role = "";

            if (String.Equals(serviceProvider.password, password))
            {
                
                if (serviceProvider.isAdmin)
                {
                    role = "Admin";
                }
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, serviceProvider.id.ToString()),
                    new Claim(ClaimTypes.Name, serviceProvider.name),
                    new Claim(ClaimTypes.Email, serviceProvider.email),
                    new Claim(ClaimTypes.Role, role)
                };
                var claimsIdentity = new ClaimsIdentity(claims, "MyAuthScheme");
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(1)
                };

                await _httpContextAccessor.HttpContext.SignInAsync("MyAuthScheme", new ClaimsPrincipal(claimsIdentity), authProperties);

                //redirect here
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["message"] = "Incorrect credentials! Try again";
                return View();
            }

            
        }

    }
}
