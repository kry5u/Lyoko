using Microsoft.AspNetCore.Mvc;
using Lyoko.Models;
using System.Diagnostics;

namespace ProgramowanieZaawansowane.Controllers
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
            ViewData["Title"] = "Witamy w Lyoko";
            return View();
        }

        public IActionResult About()
        {
            ViewData["Title"] = "O grze";
            return View();
        }

        public IActionResult Error404()
        {
            ViewData["Title"] = "Error404";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
