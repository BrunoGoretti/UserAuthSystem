using Microsoft.AspNetCore.Mvc;
using UserAuthSystemMvc.Models;
using UserAuthSystemMvc.Services.Interfaces;

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

/*        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _authService.Authenticate(email, password);
            if (user != null)
            {
                // Manage session or authentication here
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }*/
    }
}