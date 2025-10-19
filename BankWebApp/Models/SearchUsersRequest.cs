namespace BankWebApp.Models
{
    public class SearchUsersRequest
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}