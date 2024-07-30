using Microsoft.AspNetCore.Mvc;

namespace UserAuthSystemProj.Models
{
    public class PasswordResetToken : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
