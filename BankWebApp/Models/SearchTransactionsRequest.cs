namespace BankWebApp.Models
{
    public class SearchTransactionsRequest
    {
        public int? TransactionId { get; set; }
        public int? AccountNumber { get; set; }
        public string? Type { get; set; }
    }
}