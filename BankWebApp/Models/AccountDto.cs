namespace BankWebApp.Models
{
    public class AccountDto
    {
        public int AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
