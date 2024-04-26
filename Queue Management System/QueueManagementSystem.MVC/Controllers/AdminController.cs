using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Queue_Management_System.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult ConfigureServices()
        {
            return View();
        }

        public IActionResult ConfigureServicePoints()
        {
            return View();
        }

        public IActionResult ConfigureServiceProviders()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }
    }   
}