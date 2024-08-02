using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using UserAuthSystemProj.Data;
using UserAuthSystemProj.Models;
using UserAuthSystemProj.Services;
using UserAuthSystemProj.Services.Interfaces;

namespace UserAuthSystemProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public readonly AppDbContext _context;
        public readonly IAuthService _authService;

        public AuthController (AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        [HttpPost("registration")]
        public async Task<ActionResult<UserModel>> Registration(string email, string username, string password )
        {
            UserModel newRegistration = new UserModel
            {
                Email = email,
                Username = username,
                PasswordHash = password
            };

            _context.DbUsers.Add(newRegistration);
            await _context.SaveChangesAsync();

            return Ok(newRegistration);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(string email, string password)
        {
            UserModel user = await _context.DbUsers.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound("User not found");
            }

            if (user.PasswordHash != password)
            {
                return Unauthorized("Invalid password");
            }

            return Ok(user);
        }

    }
}


