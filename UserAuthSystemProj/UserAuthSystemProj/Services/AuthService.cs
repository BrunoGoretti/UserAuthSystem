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

        public async Task<UserModel> AddEmail(string userEmail)
        {
            var newEmail = new UserModel
            { 
                Email = userEmail 
            };

            _dbcontex.Add(newEmail);
            await _dbcontex.SaveChangesAsync();
            return newEmail;
        }

        public async Task<UserModel> AddUsername(string userName)
        {
            var newUserEmail = new UserModel
            {
                Username = userName
            };

            _dbcontex.Add(newUserEmail);
            await _dbcontex.SaveChangesAsync();
            return newUserEmail;
        }

        public async Task<UserModel> AddPassword (string userPassword)
        {
            var newEmail = new UserModel
            {
                Email = userPassword
            };
            _dbcontex.Add(newEmail);
            await _dbcontex.SaveChangesAsync();
            return newEmail;
        }
    }
}
