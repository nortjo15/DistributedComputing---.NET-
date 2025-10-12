namespace BankWebService.Models
{
    public class UserProfile
    {
        // need exception handling on duplicate Usernames
        public string Username { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte[] Picture { get; set; }
        public string Password { get; set; }
    }
}