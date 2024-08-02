using UserAuthSystemMvc.Models;

namespace UserAuthSystemMvc.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel> AddUser(UserModel user);
    }
}
