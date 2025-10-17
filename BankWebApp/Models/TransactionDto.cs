namespace BankWebApp.Models
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = string.Empty; // expects "Deposit" or "Withdraw"
    }
}
