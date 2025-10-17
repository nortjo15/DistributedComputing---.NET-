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
                    { "barbararoberts109", "29 Roberts Street, City 7", "barbara_roberts@email.com", "pass2691", "0467060979", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "josephadams360", "140 Adams Street, City 1", "joseph_adams@email.com", "pass9813", "0432004499", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "robertbaker240", "109 Baker Street, City 6", "robert_baker@email.com", "pass6132", "0428432091", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "roberthall206", "43 Hall Street, City 7", "robert_hall@email.com", "pass9037", "0473506488", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "robertparker737", "159 Parker Street, City 6", "robert_parker@email.com", "pass7172", "0442074826", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Username" },
                values: new object[,]
                {
                    { 1, 9769m, "robert_hall@email.com", "roberthall206" },
                    { 2, 7181m, "robert_hall@email.com", "roberthall206" },
                    { 3, 260m, "robert_baker@email.com", "robertbaker240" },
                    { 4, 9183m, "robert_baker@email.com", "robertbaker240" },
                    { 5, 3631m, "robert_baker@email.com", "robertbaker240" },
                    { 6, 800m, "robert_parker@email.com", "robertparker737" },
                    { 7, 8742m, "robert_parker@email.com", "robertparker737" },
                    { 8, 7645m, "robert_parker@email.com", "robertparker737" },
                    { 9, 547m, "joseph_adams@email.com", "josephadams360" },
                    { 10, 5763m, "barbara_roberts@email.com", "barbararoberts109" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 154m, "Deposit" },
                    { 2, 1, 122m, "Deposit" },
                    { 3, 1, 499m, "Deposit" },
                    { 4, 1, 370m, "Deposit" },
                    { 5, 2, 216m, "Deposit" },
                    { 6, 2, 108m, "Withdraw" },
                    { 7, 3, 941m, "Withdraw" },
                    { 8, 3, 754m, "Withdraw" },
                    { 9, 3, 470m, "Deposit" },
                    { 10, 3, 86m, "Deposit" },
                    { 11, 4, 455m, "Deposit" },
                    { 12, 4, 888m, "Deposit" },
                    { 13, 4, 959m, "Withdraw" },
                    { 14, 4, 421m, "Withdraw" },
                    { 15, 5, 579m, "Withdraw" },
                    { 16, 5, 717m, "Deposit" },
                    { 17, 5, 385m, "Deposit" },
                    { 18, 6, 820m, "Deposit" },
                    { 19, 6, 499m, "Deposit" },
                    { 20, 6, 462m, "Withdraw" },
                    { 21, 6, 823m, "Withdraw" },
                    { 22, 6, 320m, "Deposit" },
                    { 23, 6, 951m, "Deposit" },
                    { 24, 6, 266m, "Deposit" },
                    { 25, 6, 731m, "Deposit" },
                    { 26, 6, 903m, "Deposit" },
                    { 27, 7, 185m, "Withdraw" },
                    { 28, 7, 652m, "Withdraw" },
                    { 29, 7, 686m, "Deposit" },
                    { 30, 7, 288m, "Deposit" },
                    { 31, 7, 425m, "Withdraw" },
                    { 32, 8, 518m, "Deposit" },
                    { 33, 8, 23m, "Withdraw" },
                    { 34, 8, 992m, "Deposit" },
                    { 35, 8, 665m, "Withdraw" },
                    { 36, 8, 621m, "Withdraw" },
                    { 37, 8, 969m, "Deposit" },
                    { 38, 8, 502m, "Withdraw" },
                    { 39, 8, 146m, "Deposit" },
                    { 40, 8, 776m, "Withdraw" },
                    { 41, 9, 493m, "Deposit" },
                    { 42, 9, 59m, "Withdraw" },
                    { 43, 9, 851m, "Withdraw" },
                    { 44, 9, 690m, "Withdraw" },
                    { 45, 9, 591m, "Deposit" },
                    { 46, 9, 333m, "Deposit" },
                    { 47, 9, 906m, "Deposit" },
                    { 48, 9, 797m, "Withdraw" },
                    { 49, 10, 698m, "Deposit" },
                    { 50, 10, 279m, "Withdraw" },
                    { 51, 10, 856m, "Withdraw" },
                    { 52, 10, 356m, "Withdraw" },
                    { 53, 10, 860m, "Deposit" },
                    { 54, 10, 64m, "Withdraw" },
                    { 55, 10, 887m, "Withdraw" },
                    { 56, 10, 652m, "Deposit" }
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
