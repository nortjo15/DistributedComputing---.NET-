using System.ComponentModel.DataAnnotations;

namespace BankWebService.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        // Optional: if provided, will be verified
        public string? CurrentPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
