using BankWebService.Models;
using Microsoft.EntityFrameworkCore;

namespace BankWebService.Data
{
    public static class SeedData
    {
        public static async Task EnsureAdminAsync(DBManager db)
        {
            // Apply migrations and ensure database exists
            await db.Database.MigrateAsync();

            var adminUsername = "BankAdmin";
            var exists = await db.UserProfiles.AsNoTracking().AnyAsync(u => u.Username == adminUsername);
            if (exists) return;

            var admin = new UserProfile
            {
                Username = adminUsername,
                Email = "admin@bank.local",
                Address = "Head Office",
                Phone = "+0000000000",
                Picture = "/images/admin.png",
                Password = "ChangeMe!123"
            };

            db.UserProfiles.Add(admin);
            await db.SaveChangesAsync();
        }
    }
}
