using Microsoft.AspNetCore.Mvc;

namespace UserAuthSystemProj.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
