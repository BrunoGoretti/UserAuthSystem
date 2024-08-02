using Microsoft.AspNetCore.Mvc;
using UserAuthSystemMvc.Models;
using UserAuthSystemMvc.Services.Interfaces;
using System.Threading.Tasks;

namespace UserAuthSystemMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(UserModel model)
        {
            if (ModelState.IsValid)
            {
                await _authService.AddUser(model);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _authService.AuthenticateUser(model.Email, model.Password);
                if (user != null)
                {
                    // Add authentication logic here, e.g., set cookies or session
                    Console.WriteLine("Gji");
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }
    }
}