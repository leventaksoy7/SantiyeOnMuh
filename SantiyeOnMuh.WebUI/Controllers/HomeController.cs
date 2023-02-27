using Microsoft.AspNetCore.Mvc;
using SantiyeOnMuh.Business.Abstract;
using SantiyeOnMuh.WebUI.Models;
using System.Diagnostics;

namespace SantiyeOnMuh.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        //INJ
        private ISantiyeService _santiyeService;
        public HomeController(ISantiyeService santiyeService)
        {
            _santiyeService = santiyeService;
        }
        //
        public IActionResult Methodmethod(ISantiyeService santiyeService)
        {
            

            return View();
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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