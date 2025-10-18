using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BankWebService.Migrations
{
    /// <inheritdoc />
    public partial class SeedRefresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User_Profiles",
                columns: table => new
                {
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false),
                    Picture = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Profiles", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Bank_Accounts",
                columns: table => new
                {
                    AccountNumber = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Balance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank_Accounts", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Bank_Accounts_User_Profiles_Username",
                        column: x => x.Username,
                        principalTable: "User_Profiles",
                        principalColumn: "Username",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AccountNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.CheckConstraint("CK_Transaction_Type", "Type IN ('Deposit','Withdraw')");
                    table.ForeignKey(
                        name: "FK_Transactions_Bank_Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Bank_Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "User_Profiles",
                columns: new[] { "Username", "Address", "Email", "Password", "Phone", "Picture" },
                values: new object[,]
                {
                    { "BankAdmin", "Head Office", "admin@bank.local", "ChangeMe!123", "+0000000000", "/images/admin.png" },
                    { "BankAdmin2", "Head Office", "admin2@bank.local", "ChangeMe!123", "+0000000000", "/images/admin.png" },
                    { "BankAdmin3", "Head Office", "admin3@bank.local", "ChangeMe!123", "+0000000000", "/images/admin.png" },
                    { "karencollins863", "130 Collins Street, City 2", "karen_collins@email.com", "pass9615", "0496715500", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "karennelson920", "111 Nelson Street, City 2", "karen_nelson@email.com", "pass4169", "0468909782", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "sarahdiaz875", "192 Diaz Street, City 7", "sarah_diaz@email.com", "pass3481", "0475742900", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "susancarter997", "88 Carter Street, City 5", "susan_carter@email.com", "pass2035", "0441090544", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "thomasroberts138", "106 Roberts Street, City 2", "thomas_roberts@email.com", "pass2992", "0461647360", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Username" },
                values: new object[,]
                {
                    { 1, 6011m, "karen_collins@email.com", "karencollins863" },
                    { 2, 4893m, "thomas_roberts@email.com", "thomasroberts138" },
                    { 3, 7585m, "thomas_roberts@email.com", "thomasroberts138" },
                    { 4, 8037m, "thomas_roberts@email.com", "thomasroberts138" },
                    { 5, 8000m, "sarah_diaz@email.com", "sarahdiaz875" },
                    { 6, 8818m, "sarah_diaz@email.com", "sarahdiaz875" },
                    { 7, 1191m, "sarah_diaz@email.com", "sarahdiaz875" },
                    { 8, 8024m, "susan_carter@email.com", "susancarter997" },
                    { 9, 3976m, "susan_carter@email.com", "susancarter997" },
                    { 10, 2501m, "susan_carter@email.com", "susancarter997" },
                    { 11, 4135m, "karen_nelson@email.com", "karennelson920" },
                    { 12, 2711m, "karen_nelson@email.com", "karennelson920" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 558m, "Deposit" },
                    { 2, 1, 162m, "Withdraw" },
                    { 3, 1, 53m, "Withdraw" },
                    { 4, 1, 52m, "Deposit" },
                    { 5, 1, 902m, "Deposit" },
                    { 6, 1, 701m, "Deposit" },
                    { 7, 1, 895m, "Deposit" },
                    { 8, 1, 829m, "Deposit" },
                    { 9, 1, 98m, "Deposit" },
                    { 10, 2, 470m, "Deposit" },
                    { 11, 2, 657m, "Deposit" },
                    { 12, 2, 501m, "Withdraw" },
                    { 13, 3, 715m, "Withdraw" },
                    { 14, 3, 924m, "Withdraw" },
                    { 15, 3, 713m, "Deposit" },
                    { 16, 3, 148m, "Deposit" },
                    { 17, 3, 374m, "Deposit" },
                    { 18, 4, 349m, "Deposit" },
                    { 19, 4, 350m, "Deposit" },
                    { 20, 4, 64m, "Withdraw" },
                    { 21, 4, 927m, "Withdraw" },
                    { 22, 5, 198m, "Deposit" },
                    { 23, 5, 559m, "Deposit" },
                    { 24, 5, 382m, "Withdraw" },
                    { 25, 6, 790m, "Withdraw" },
                    { 26, 6, 131m, "Deposit" },
                    { 27, 6, 414m, "Deposit" },
                    { 28, 6, 573m, "Withdraw" },
                    { 29, 6, 775m, "Deposit" },
                    { 30, 6, 656m, "Deposit" },
                    { 31, 6, 811m, "Withdraw" },
                    { 32, 6, 252m, "Withdraw" },
                    { 33, 6, 104m, "Deposit" },
                    { 34, 7, 814m, "Withdraw" },
                    { 35, 7, 752m, "Withdraw" },
                    { 36, 7, 354m, "Withdraw" },
                    { 37, 7, 542m, "Deposit" },
                    { 38, 7, 650m, "Deposit" },
                    { 39, 8, 378m, "Withdraw" },
                    { 40, 8, 544m, "Deposit" },
                    { 41, 8, 126m, "Withdraw" },
                    { 42, 8, 197m, "Deposit" },
                    { 43, 9, 194m, "Withdraw" },
                    { 44, 9, 109m, "Deposit" },
                    { 45, 10, 512m, "Deposit" },
                    { 46, 10, 526m, "Withdraw" },
                    { 47, 10, 928m, "Withdraw" },
                    { 48, 10, 102m, "Withdraw" },
                    { 49, 10, 386m, "Deposit" },
                    { 50, 11, 353m, "Deposit" },
                    { 51, 12, 273m, "Withdraw" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bank_Accounts_Username",
                table: "Bank_Accounts",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountNumber",
                table: "Transactions",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_User_Profiles_Email",
                table: "User_Profiles",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Bank_Accounts");

            migrationBuilder.DropTable(
                name: "User_Profiles");
        }
    }
}
