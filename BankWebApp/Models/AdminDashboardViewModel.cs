using System.Collections.Generic;

namespace BankWebApp.Models
{
    public class AdminDashboardViewModel
    {
        public List<UserProfileDto> Users { get; set; } = new();
        public List<TransactionDto> Transactions { get; set; } = new();
        public List<AccountDto> Accounts { get; set; } = new();
        public UserProfileDto? Admin { get; set; }
    }
}
