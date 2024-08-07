using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserAuthSystemMvc.Data;
using UserAuthSystemMvc.Models;
using UserAuthSystemMvc.Services.Interfaces;

namespace UserAuthSystemMvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;

        public AuthService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserModel> AddUser(UserModel user)
        {
            var newUser = new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                PasswordHash = GenerateHashed(user.PasswordHash),
                HashedId = GenerateHashed(user.Email)
            };

            _dbContext.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return newUser;
        }

        private string GenerateHashed(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public async Task<UserModel> AuthenticateUser(string email, string password)
        {
            var user = await _dbContext.DbUsers
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || user.PasswordHash != GenerateHashed(password))
            {
                return null;
            }

            return user;
        }
    }
}