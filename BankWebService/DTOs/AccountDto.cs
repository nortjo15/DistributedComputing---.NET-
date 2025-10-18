namespace BankWebService.DTOs
{
    public class AccountDto
    {
        public int AccountNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
