namespace BankWebService.Models
{
    public class UserProfile
    {
        // need exception handling on duplicate Usernames
        public string Username { get; set; } = null!;        //PK
        public string Email { get; set; } = null!;           //Unique
        public string Address { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Picture { get; set; } = null!;         //Save as url to file 
        public string Password { get; set; } = null!;

        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}