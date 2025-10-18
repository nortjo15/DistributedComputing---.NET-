namespace BankWebService.DTOs
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
