using DormitoryApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DormitoryApplication.Controllers
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
            return View();
        }

        public IActionResult Giris()
        {
            return View();
        }

        public IActionResult Odeme()
        {
            return View();
        }
        public IActionResult Sifreunuttum()
        {
            return View();
        }
        public IActionResult Sifreunuttum2()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Admin_yurt_secenekler()
        {
            return View();
        }
        public IActionResult Admin_talepler()
        {
            return View();
        }
        public IActionResult Admin_talep_detay()
        {
            return View();
        }
        public IActionResult Admin_oda()
        {
            return View();
        }
        public IActionResult Dorm_apply()
        {
            return View();
        }
        public IActionResult Talepler()
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