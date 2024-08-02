using UserAuthSystemProj.Data;
using UserAuthSystemProj.Models;
using UserAuthSystemProj.Services.Interfaces;

namespace UserAuthSystemProj.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbcontex;

        public AuthService(AppDbContext dbcontex) 
        {
            _dbcontex = dbcontex;
        }

        public async Task<UserModel> AddUser(UserModel user)
        {
            var newUser = new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                PasswordHash = user.PasswordHash,
            };
            _dbcontex.Add(newUser);
            await _dbcontex.SaveChangesAsync();
            return newUser;
        }
    }
}
