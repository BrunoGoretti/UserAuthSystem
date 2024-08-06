using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserAuthSystemMvc.Attributes
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Password is required.");
            }

            if (password.Length < 6)
            {
                return new ValidationResult("Password must be at least 6 characters long.");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Password must contain at least one uppercase letter.");
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?\"":{}|<>]"))
            {
                return new ValidationResult("Password must contain at least one special character.");
            }

            return ValidationResult.Success;
        }
    }
}