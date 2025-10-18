namespace BankWebService.Models
{
    public class Account
    {
        public int AccountNumber { get; set; }      //PK
        public decimal Balance { get; set; }
        public string Username { get; set; } = null!;    //FK to UserProfile
        public string? Email { get; set; }

        // Navigation properties should be optional for binding/serialization
        public UserProfile? UserProfile { get; set; }
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
