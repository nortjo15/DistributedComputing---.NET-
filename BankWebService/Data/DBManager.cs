using BankWebService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BankWebService.Data
{
    public class DBManager : DbContext
    {
        private const int numberOfUsers = 5;    //Number of users seeded into database
        private const int numberOfIcons = 5;    //How many profile icons there are
        private const int maxAccounts = 3;      //Maximum number of accounts seeded in
        private const int maxTransactions = 10; //Maximum number of transactions seeded in

        public DBManager(DbContextOptions<DBManager> options) : base(options) { }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public override DatabaseFacade Database => base.Database;

        public override ChangeTracker ChangeTracker => base.ChangeTracker;

        public override IModel Model => base.Model;

        public override DbContextId ContextId => base.ContextId;

        // Seed x accounts, y transactions, z user profiles
        // Challenge here is in linking the creation of everything eg.
        // Each Account will need to be linked to a UserProfile, then will have to be populated in the SQLite DB
        // Same for Transactions - need to be stored in DB
        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<UserProfile>(e =>
            {
                e.ToTable("User_Profiles");
                e.HasKey(x => x.Username);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Address).HasMaxLength(200);
                e.Property(x => x.Phone).HasMaxLength(15);
                e.Property(x => x.Picture).HasMaxLength(500);
                e.Property(x => x.Password).HasMaxLength(30).IsRequired();
            });

            mb.Entity<Account>(e =>
            {
                e.ToTable("Bank_Accounts");
                e.HasKey(x => x.AccountNumber);
                e.Property(x => x.Balance).IsRequired();
                e.HasOne(a => a.UserProfile)
                    .WithMany(u => u.Accounts)
                    .HasForeignKey(a => a.Username)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            mb.Entity<Transaction>(e =>
            {
                e.ToTable("Transactions", tb => tb.HasCheckConstraint(
                    "CK_Transaction_Type",
                    "Type IN ('Deposit','Withdraw')"));

                e.HasKey(t => t.TransactionId);
                e.Property(t => t.Amount).IsRequired();
                e.Property(t => t.Type).HasMaxLength(10).IsRequired().HasConversion<string>();

                e.HasOne(t => t.Account)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(t => t.AccountNumber)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            /* ---------- DATA SEEDING ----------*/
            var rand = new Random();

            var firstNames = new List<string>
            {
                "James","Michael","John","Robert","David","William","Richard",
                "Joseph","Thomas","Christopher","Mary","Patricia","Jennifer",
                "Linda","Elizabeth","Barbara","Susan","Jessica","Karen","Sarah"
            };

            var lastNames = new List<string>
            {
                "Flores","Green","Adams","Nelson","Baker","Hall","Rivera","Campbell",
                "Mitchell","Carter","Roberts","Gomez","Phillips","Evans","Turner",
                "Diaz","Parker","Cruz","Edwards","Collins","Reyes"
            };

            /* ----- profile picture folder (dynamic) ----- */
            string projectRoot = AppContext.BaseDirectory;

            // Go up to the solution root and into resources/ProfilePictures
            string pictureRoot = Path.GetFullPath(
                Path.Combine(projectRoot, "..", "..", "..", "..", "resources", "ProfilePictures")
            );

            // Build list manually since pictures are named 1.jpg through 5.jpg
            var availablePics = Enumerable.Range(1, numberOfIcons)
                .Select(i => Path.Combine(pictureRoot, $"{i}.jpg"))
                .Where(File.Exists)
                .ToArray();

            // Fallback if no pictures found. Uses default.jpg
            if (availablePics.Length == 0)
            {
                availablePics = new[] { Path.Combine(pictureRoot, "default.jpg") };
            }

            int accountIdCounter = 1;
            int transactionIdCounter = 1;

            var users = new List<UserProfile>();
            var accounts = new List<Account>();
            var transactions = new List<Transaction>();

            for (int i = 0; i < numberOfUsers; i++)
            {
                string first = firstNames[rand.Next(firstNames.Count)];
                string last = lastNames[rand.Next(lastNames.Count)];
                string username = $"{first.ToLower()}{last.ToLower()}{rand.Next(100, 999)}";
                string email = $"{first.ToLower()}_{last.ToLower()}@email.com";

                // pick random picture
                string picture = availablePics[rand.Next(availablePics.Length)];

                var user = new UserProfile
                {
                    Username = username,
                    Email = email,
                    Address = $"{rand.Next(1, 200)} {last} Street, City {rand.Next(1, 10)}",
                    Phone = $"04{rand.Next(10000000, 99999999)}",
                    Picture = picture,
                    Password = $"pass{rand.Next(1000, 9999)}"
                };
                users.Add(user);

                // How many accounts for this user
                int numAccounts = rand.Next(1, maxAccounts + 1);
                for (int a = 0; a < numAccounts; a++)
                {
                    var account = new Account
                    {
                        AccountNumber = accountIdCounter,
                        Username = user.Username,
                        Balance = rand.Next(100, 10000),
                        Email = user.Email
                    };
                    accounts.Add(account);

                    // How many transactions for this account
                    int numTransactions = rand.Next(1, maxTransactions);
                    for (int t = 0; t < numTransactions; t++)
                    {
                        var txType = (rand.Next(2) == 0)
                            ? Transaction.TxType.Deposit
                            : Transaction.TxType.Withdraw;

                        transactions.Add(new Transaction
                        {
                            TransactionId = transactionIdCounter++,
                            AccountNumber = account.AccountNumber,
                            Type = txType,
                            Amount = rand.Next(10, 1000)
                        });
                    }

                    accountIdCounter++;
                }
            }

            // ----- apply seed data -----
            mb.Entity<UserProfile>().HasData(users);
            mb.Entity<Account>().HasData(accounts);
            mb.Entity<Transaction>().HasData(transactions);
        }


        public override DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>()
        {
            return base.Set<TEntity>();
        }

        public override DbSet<TEntity> Set<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(string name)
        {
            return base.Set<TEntity>(name);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            return base.DisposeAsync();
        }

        public override EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        {
            return base.Entry(entity);
        }

        public override EntityEntry Entry(object entity)
        {
            return base.Entry(entity);
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return base.Add(entity);
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        {
            return base.Attach(entity);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            return base.Update(entity);
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            return base.Remove(entity);
        }

        public override EntityEntry Add(object entity)
        {
            return base.Add(entity);
        }

        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            return base.AddAsync(entity, cancellationToken);
        }

        public override EntityEntry Attach(object entity)
        {
            return base.Attach(entity);
        }

        public override EntityEntry Update(object entity)
        {
            return base.Update(entity);
        }

        public override EntityEntry Remove(object entity)
        {
            return base.Remove(entity);
        }

        public override void AddRange(params object[] entities)
        {
            base.AddRange(entities);
        }

        public override Task AddRangeAsync(params object[] entities)
        {
            return base.AddRangeAsync(entities);
        }

        public override void AttachRange(params object[] entities)
        {
            base.AttachRange(entities);
        }

        public override void UpdateRange(params object[] entities)
        {
            base.UpdateRange(entities);
        }

        public override void RemoveRange(params object[] entities)
        {
            base.RemoveRange(entities);
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            base.AddRange(entities);
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            return base.AddRangeAsync(entities, cancellationToken);
        }

        public override void AttachRange(IEnumerable<object> entities)
        {
            base.AttachRange(entities);
        }

        public override void UpdateRange(IEnumerable<object> entities)
        {
            base.UpdateRange(entities);
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            base.RemoveRange(entities);
        }

        public override object? Find([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues)
        {
            return base.Find(entityType, keyValues);
        }

        public override ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, params object?[]? keyValues)
        {
            return base.FindAsync(entityType, keyValues);
        }

        public override ValueTask<object?> FindAsync([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] Type entityType, object?[]? keyValues, CancellationToken cancellationToken)
        {
            return base.FindAsync(entityType, keyValues, cancellationToken);
        }

        public override TEntity? Find<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class
        {
            return base.Find<TEntity>(keyValues);
        }

        public override ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(params object?[]? keyValues) where TEntity : class
        {
            return base.FindAsync<TEntity>(keyValues);
        }

        public override ValueTask<TEntity?> FindAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.NonPublicFields | DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.NonPublicProperties | DynamicallyAccessedMemberTypes.Interfaces)] TEntity>(object?[]? keyValues, CancellationToken cancellationToken) where TEntity : class
        {
            return base.FindAsync<TEntity>(keyValues, cancellationToken);
        }

        public override IQueryable<TResult> FromExpression<TResult>(Expression<Func<IQueryable<TResult>>> expression)
        {
            return base.FromExpression(expression);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
