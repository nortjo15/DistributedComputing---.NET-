namespace BankWebService.DTOs
{
    public class CreateAccountRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string? Email { get; set; }
    }
}