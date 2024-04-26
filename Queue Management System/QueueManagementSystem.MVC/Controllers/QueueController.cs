using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementSystem.MVC.Services;

namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public QueueController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet]
        public async Task<IActionResult> CheckinPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult WaitingPage()
        {
            return View();
        }
        
        [Authorize, HttpGet]
        public IActionResult ServicePoint()
        {
            return View();
        }


    }
}