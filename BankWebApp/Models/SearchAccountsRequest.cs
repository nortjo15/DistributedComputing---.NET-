namespace BankWebApp.Models
{
    public class SearchAccountsRequest
    {
        public int? AccountNumber { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
    }
}