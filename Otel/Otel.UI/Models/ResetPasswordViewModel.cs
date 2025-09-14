using System.ComponentModel.DataAnnotations;

namespace Otel.UI.Models
{
    public class ResetPasswordViewModel
    {
        public  string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password do not match")]
        public  string? ConfirmPassword { get; set; }
        public required string ResetToken { get; set; }
        public  string? Email { get; set; }

    }
}
