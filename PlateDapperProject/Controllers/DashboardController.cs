using Microsoft.AspNetCore.Mvc;

namespace PlateDapperProject.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
