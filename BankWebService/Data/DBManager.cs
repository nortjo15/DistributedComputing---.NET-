using BankWebService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace BankWebService.Data
{
    public class DBManager : DbContext
    {
        private int accountNumber = 100;
        private int transactionNumber = 100;
        private int profileNumber = 100;

        // Connection string used by the Create*Table methods
        // Force the database file to live in the project root (3 levels up from bin/Debug/net6.0)
        private static readonly string DbFileName = "mydatabase.db";
        private static readonly string DbFullPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", DbFileName));
        private static readonly string accountsConnectionString = $"Data Source={DbFullPath};";
        
        // Create table (UserProfiles)
        public static bool CreateUserProfileTable()
        {
            try
            {
                using (var connection = new SqliteConnection(accountsConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        // SQL command to create a table named "Accounts"
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS UserProfiles (
                        Username TEXT,
                        Email TEXT,
                        Address TEXT,
                        Phone TEXT,
                        Picture BLOB,
                        Password TEXT NOT NULL,
                        PRIMARY KEY (Username, Email)
                        );";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Console.WriteLine("UserProfile table created successfully.");
                Console.WriteLine($"DB created: True. Path: {DbFullPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Create table failed
            }
        }

        // Create table (Accounts)
        public static bool CreateAccountsTable()
        {
            try
            {
                using (var connection = new SqliteConnection(accountsConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        // SQL command to create a table named "Accounts"
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Accounts (
                        AccountNumber INTEGER PRIMARY KEY,
                        Balance REAL NOT NULL DEFAULT 0,
                        Username TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        FOREIGN KEY (Username) REFERENCES UserProfiles(Username)
                            ON UPDATE CASCADE ON DELETE RESTRICT
                        );";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Console.WriteLine("Accounts table created successfully.");
                Console.WriteLine($"DB created: True. Path: {DbFullPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Create table failed
            }
        }

        // Create table (transactions)
        public static bool CreateTransactionsTable()
        {
            try
            {
                using (var connection = new SqliteConnection(accountsConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        // SQL command to create a table named "Transactions"
                        command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS Transactions (
                        TransactionId INTEGER PRIMARY KEY,
                        AccountNumber INTEGER NOT NULL,
                        Type TEXT NOT NULL CHECK (Type IN ('Deposit','Withdraw')),
                        Amount REAL NOT NULL,
                        FOREIGN KEY (AccountNumber) REFERENCES Accounts(AccountNumber)
                            ON UPDATE CASCADE ON DELETE CASCADE
                        );";

                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }

                Console.WriteLine("Transactions table created successfully.");
                Console.WriteLine($"DB created: True. Path: {DbFullPath}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false; // Create table failed
            }
        }

        // Insert a new account row
        public static bool CreateAccount(int acctNum, Account account)
        {
            try
            {
                using (var connection = new SqliteConnection(accountsConnectionString))
                {
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        // SQL command to insert data into the "Accounts" table
                        command.CommandText = @"
                        INSERT INTO Accounts (AccountNumber, Balance, Username, Email)
                        VALUES (@AccountNumber, @Balance, @Username, @Email);";

                        // Define parameters for the query
                        command.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                        command.Parameters.AddWithValue("@Balance", account.Balance);
                        command.Parameters.AddWithValue("@Username", account.Username);
                        command.Parameters.AddWithValue("@Email", account.Email);

                        // Execute the SQL command to insert data
                        int rowsInserted = command.ExecuteNonQuery();
                        connection.Close();

                        // Check if any rows were inserted
                        if (rowsInserted > 0)
                        {
                            return true; // Insertion was successful
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return false; // Insertion failed
        }

        //Use this func to reset the database during testing
        public static bool RecreateSchema()
        {
            try
            {
                using var connection = new SqliteConnection(accountsConnectionString);
                connection.Open();

                using (var drop = connection.CreateCommand())
                {
                    drop.CommandText = @"
                    PRAGMA foreign_keys = OFF;
                    DROP TABLE IF EXISTS Transactions;
                    DROP TABLE IF EXISTS Accounts;
                    DROP TABLE IF EXISTS UserProfiles;
                    PRAGMA foreign_keys = ON;";
                    drop.ExecuteNonQuery();
                }

                var ok = CreateUserProfileTable();
                ok &= CreateAccountsTable();
                ok &= CreateTransactionsTable();
                return ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error recreating schema: " + ex.Message);
                return false;
            }
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source = Accounts.db;");
            optionsBuilder.UseSqlite(@"Data Source = UserProfiles.db;");
            optionsBuilder.UseSqlite(@"Data Source = Transactions.db;");
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Seed x accounts, y transactions, z user profiles
        // Challenge here is in linking the creation of everything eg.
        // Each Account will need to be linked to a UserProfile, then will have to be populated in the SQLite DB
        // Same for Transactions - need to be stored in DB
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Random random = new Random();

            List<Account> accounts = new List<Account>();
            List<Transaction> transactions = new List<Transaction>();
            List<UserProfile> userprofiles = new List<UserProfile>();

            for (int i = 1; i <= profileNumber; i++)
            {
                // TODO complete user profile generation w/ all fields
                UserProfile up = new UserProfile
                {
                    Username = $"user{i}",
                    Email = $"user{i}",
                };
            }

            for (int i = 1; i <= accountNumber; i++)
            {
                UserProfile profile = userprofiles[random.Next(0, profileNumber - 1)];
                Account a = new Account
                {
                    AccountNumber = i,
                    Balance = Math.Round(random.NextDouble() * random.Next(0, 1000000), 2),
                    Username = profile.Username,
                    Email = profile.Email
                };

                accounts.Add(a);
            }

            string[] transactionTypes = { "Deposit", "Withdrawal" };
            for (int i = 1; i <= transactionNumber; i++)
            {
                Account account = accounts[random.Next(0, accountNumber - 1)];
                Transaction t = new Transaction
                {
                    TransactionId = i,
                    AccountNumber = account.AccountNumber,
                    Amount = Math.Round(random.NextDouble() * random.Next(0, 10000), 2),
                    Type = transactionTypes[random.Next(0, 1)],
                };
                transactions.Add(t);
            }

            // Populate the DB w/ the generated data
            modelBuilder.Entity<UserProfile>().HasData(userprofiles);
            modelBuilder.Entity<Account>().HasData(accounts);
            modelBuilder.Entity<Transaction>().HasData(transactions);
        }
    }
}
