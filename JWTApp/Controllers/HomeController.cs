using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JWTApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int id)
        {
            return View();
        }


    }
}

