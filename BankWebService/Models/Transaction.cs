namespace BankWebService.Models
{
    // Pseudo Schema for Transaction
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int AccountNumber { get; set; }
        public string Type { get; set; } // "Deposit" or "Withdraw"
        public double Amount { get; set; } // Represented w/ only positive numbers
    }
}