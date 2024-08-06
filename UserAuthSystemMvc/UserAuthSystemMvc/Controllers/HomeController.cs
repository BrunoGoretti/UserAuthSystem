using Microsoft.AspNetCore.Mvc;

namespace UserAuthSystemMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
