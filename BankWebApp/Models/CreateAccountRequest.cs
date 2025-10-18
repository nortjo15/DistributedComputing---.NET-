namespace BankWebApp.Models
{
    public class CreateAccountRequest
    {
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
        public decimal Balance { get; set; }
    }
}
