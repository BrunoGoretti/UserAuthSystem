using UserAuthSystemMvc.Models;

namespace UserAuthSystemMvc.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel> AddUser(UserModel user);
        Task<UserModel> AuthenticateUser(string email, string password);

    }
}
