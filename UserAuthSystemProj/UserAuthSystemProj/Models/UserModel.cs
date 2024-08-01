using Microsoft.AspNetCore.Mvc;

namespace UserAuthSystemProj.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
