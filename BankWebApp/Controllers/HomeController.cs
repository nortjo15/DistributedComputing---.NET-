using BankWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}