using Microsoft.AspNetCore.Mvc;

namespace QLKS.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Rooms()
        {
            return View();
        }

        public IActionResult Bookings()
        {
            return View();
        }

        public IActionResult Customers()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
