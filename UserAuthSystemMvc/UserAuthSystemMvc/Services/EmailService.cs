using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using UserAuthSystemMvc.Services.Interfaces;

namespace UserAuthSystemMvc.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var emailAddress = _configuration["EmailSettings:EmailAddress"];
            var emailPassword = _configuration["EmailSettings:EmailPassword"];

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("UserAuthSystem", emailAddress));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(emailAddress, emailPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string token)
        {
            var resetLink = $"{_configuration["AppSettings:BaseUrl"]}https://localhost:7030/Auth/CreateNewPassword?token={token}";
            var subject = "Password Reset Request";
            var message = $"Please reset your password by clicking the following link: {resetLink}";

            await SendEmailAsync(toEmail, subject, message);
        }
    }
}