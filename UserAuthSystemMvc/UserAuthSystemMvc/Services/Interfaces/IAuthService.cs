using UserAuthSystemProj.Models;

namespace UserAuthSystemProj.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserModel> AddUser(UserModel user);
    }
}
