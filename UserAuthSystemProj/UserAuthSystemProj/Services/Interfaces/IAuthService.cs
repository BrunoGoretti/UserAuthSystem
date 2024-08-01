using UserAuthSystemProj.Models;

namespace UserAuthSystemProj.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel> AddEmail(string email);
        Task<UserModel> AddUsername(string name);
        Task<UserModel> AddPassword(string password);
    }
}
