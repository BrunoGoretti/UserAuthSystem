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

        public async Task<string> CreatePasswordResetToken(string email)
        {
            // Remove any existing tokens for the email
            var existingTokens = _dbContext.DbPasswordResetTokens.Where(t => t.Email == email);
            _dbContext.DbPasswordResetTokens.RemoveRange(existingTokens);

            // Generate new token and set expiry date
            var token = GenerateHashed(Guid.NewGuid().ToString());
            var expiryDate = DateTime.UtcNow.AddSeconds(60);

            // Create new token model
            var resetToken = new PasswordResetTokenModel
            {
                Email = email,
                Token = token,
                ExpiryDate = expiryDate
            };

            // Add new token to the database
            _dbContext.DbPasswordResetTokens.Add(resetToken);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        public async Task<PasswordResetTokenModel> GetResetToken(string token)
        {
            return await _dbContext.DbPasswordResetTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.ExpiryDate > DateTime.UtcNow);
        }

        public async Task<bool> UpdatePassword(string email, string newPassword)
        {
            var user = await _dbContext.DbUsers.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return false;
            }

            user.PasswordHash = GenerateHashed(newPassword);
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}