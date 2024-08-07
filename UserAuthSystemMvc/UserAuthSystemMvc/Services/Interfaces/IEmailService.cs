using System.Threading.Tasks;

namespace UserAuthSystemMvc.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendPasswordResetEmailAsync(string toEmail, string token);
    }
}