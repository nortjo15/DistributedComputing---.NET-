namespace BankWebService.Models
{
    // Pseudo Schema for Transaction
    public class Transaction
    {
        public enum TxType { Deposit, Withdraw }
        public int TransactionId { get; set; }      //PK
        public int AccountNumber { get; set; }      //FK to Account.Id
        public TxType Type { get; set; }            //"Deposit" or "Withdraw"
        public decimal Amount { get; set; }         //Represented w/ only positive numbers

        public Account Account { get; set; } = null!;
    }
}