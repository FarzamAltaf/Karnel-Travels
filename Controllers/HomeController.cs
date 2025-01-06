
using kernel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace kernel.Controllers
{
    public class HomeController : Controller
    {
        connection db = new connection();


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult destination()
        {
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult tour()
        {
            return View();
        }

        public IActionResult hotel()
        {
            return View();
        }

        public IActionResult visa()
        {
            return View();
        }

        public IActionResult activity()
        {
            return View();
        }

        public IActionResult transport()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

    }
}