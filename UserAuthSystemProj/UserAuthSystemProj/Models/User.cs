using Microsoft.AspNetCore.Mvc;

namespace UserAuthSystemProj.Models
{
    public class User : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
