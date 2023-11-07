using Microsoft.AspNetCore.Mvc;

namespace Simple_E_Com.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
