using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ActivitySystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ActivitySystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("SuperAdmin") || User.IsInRole("Admin"))
                return RedirectToAction("Index", "Admin");
            else if (User.IsInRole("Student"))
                return RedirectToAction("Index", "Student");
            else if (User.IsInRole("Instructor"))
                return RedirectToAction("Index", "Instructor");
            else
                return RedirectToAction("LogIn", "Account");
            
        }
 

        public IActionResult Privacy()
        {
            return View();
        }
      
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
