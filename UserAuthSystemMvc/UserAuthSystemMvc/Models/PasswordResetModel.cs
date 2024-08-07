using System.ComponentModel.DataAnnotations;

namespace UserAuthSystemMvc.Models
{
    public class PasswordResetModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}