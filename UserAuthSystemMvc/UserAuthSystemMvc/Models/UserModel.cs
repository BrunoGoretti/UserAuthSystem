using System.ComponentModel.DataAnnotations;
using UserAuthSystemMvc.Attributes;

namespace UserAuthSystemMvc.Models
{
    public class UserModel
    {
        public int Id { get; set; } 
        public string? HashedId { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [PasswordValidation]
        public string PasswordHash { get; set; }
    }
}