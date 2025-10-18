using System.ComponentModel.DataAnnotations;

namespace BankWebApp.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        // Optional: if provided, the API will verify it
        public string? CurrentPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
