using Microsoft.AspNetCore.Mvc;
using UserAuthSystemMvc.Models;
using UserAuthSystemMvc.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using UserAuthSystemMvc.Services;

namespace UserAuthSystemMvc.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IConfiguration configuration, IEmailService emailService)
        {
            _authService = authService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
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
                var user = await _authService.AddUser(model);
                var cookieExpirationInSeconds = _configuration.GetValue<int>("Authentication:CookieExpirationInSeconds");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(cookieExpirationInSeconds)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index");
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
                var cookieExpirationInSeconds = _configuration.GetValue<int>("Authentication:CookieExpirationInSeconds");
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {

                        ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(cookieExpirationInSeconds)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Invalid email or password.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult IsAuthenticated()
        {
            return Json(User.Identity.IsAuthenticated);
        }

        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset(PasswordResetModel model)
        {
            if (ModelState.IsValid)
            {
                var token = await _authService.CreatePasswordResetToken(model.Email);
                if (token != null)
                {
                    await _emailService.SendPasswordResetEmailAsync(model.Email, token);
                    ViewData["SuccessMessage"] = "Password reset email sent.";
                }
                else
                {
                    ViewData["ErrorMessage"] = "An error occurred.";
                }
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateNewPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }
            // You can also add validation for the token here
            return View(new CreateNewPasswordModel { Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewPassword(CreateNewPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var resetToken = await _authService.GetResetToken(model.Token);
                if (resetToken != null)
                {
                    // Proceed with resetting the password
                    // (Implement the logic to reset the password here)
                    // e.g., _authService.CreateNewPassword(resetToken.Email, model.NewPassword);
                    ViewData["SuccessMessage"] = "Password has been reset.";
                    return RedirectToAction("Login");
                }
                ViewData["ErrorMessage"] = "Invalid or expired token.";
            }
            return View(model);
        }
    }
}