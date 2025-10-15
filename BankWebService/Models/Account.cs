namespace BankWebService.Models
{
    public class Account
    {
        public int AccountNumber { get; set; }      //PK
        public decimal Balance { get; set; }
        public string Username { get; set; } = null!;    //FK to UserProfile
        public string? Email { get; set; }

        public UserProfile UserProfile { get; set; } = null!;
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
