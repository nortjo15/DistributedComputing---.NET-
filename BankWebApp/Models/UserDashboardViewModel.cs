namespace BankWebApp.Models
{
    public class UserDashboardViewModel
    {
        public UserProfileDto? Profile { get; set; }
        public List<AccountDto>? Accounts { get; set; }
        public List<TransactionDto>? Transactions { get; set; }
        public List<UserProfileDto>? Profiles { get; set; }
    }

}
