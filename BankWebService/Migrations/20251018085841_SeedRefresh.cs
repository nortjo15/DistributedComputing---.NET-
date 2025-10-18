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
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
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
                    { "barbaraparker204", "10 Parker Street, City 9", "barbara_parker@email.com", "pass5559", "0460433857", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "jamesdiaz996", "175 Diaz Street, City 1", "james_diaz@email.com", "pass9547", "0499003707", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "jenniferroberts143", "90 Roberts Street, City 7", "jennifer_roberts@email.com", "pass8280", "0487387878", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "jessicacampbell185", "164 Campbell Street, City 5", "jessica_campbell@email.com", "pass8105", "0451200149", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "maryhall268", "86 Hall Street, City 9", "mary_hall@email.com", "pass8613", "0453451071", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "richardhall302", "28 Hall Street, City 6", "richard_hall@email.com", "pass5181", "0461924169", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "sarahdiaz655", "110 Diaz Street, City 5", "sarah_diaz@email.com", "pass1125", "0460620077", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Name", "Username" },
                values: new object[,]
                {
                    { 1, 8863m, "jessica_campbell@email.com", "Jessica's Account 1", "jessicacampbell185" },
                    { 2, 7353m, "jessica_campbell@email.com", "Jessica's Account 2", "jessicacampbell185" },
                    { 3, 5147m, "jessica_campbell@email.com", "Jessica's Account 3", "jessicacampbell185" },
                    { 4, 2900m, "jessica_campbell@email.com", "Jessica's Account 4", "jessicacampbell185" },
                    { 5, 2064m, "james_diaz@email.com", "James's Account 1", "jamesdiaz996" },
                    { 6, 4756m, "james_diaz@email.com", "James's Account 2", "jamesdiaz996" },
                    { 7, 5408m, "james_diaz@email.com", "James's Account 3", "jamesdiaz996" },
                    { 8, 6580m, "sarah_diaz@email.com", "Sarah's Account 1", "sarahdiaz655" },
                    { 9, 1821m, "sarah_diaz@email.com", "Sarah's Account 2", "sarahdiaz655" },
                    { 10, 8267m, "sarah_diaz@email.com", "Sarah's Account 3", "sarahdiaz655" },
                    { 11, 7663m, "jennifer_roberts@email.com", "Jennifer's Account 1", "jenniferroberts143" },
                    { 12, 7236m, "richard_hall@email.com", "Richard's Account 1", "richardhall302" },
                    { 13, 1833m, "richard_hall@email.com", "Richard's Account 2", "richardhall302" },
                    { 14, 518m, "richard_hall@email.com", "Richard's Account 3", "richardhall302" },
                    { 15, 1891m, "richard_hall@email.com", "Richard's Account 4", "richardhall302" },
                    { 16, 5837m, "richard_hall@email.com", "Richard's Account 5", "richardhall302" },
                    { 17, 8716m, "mary_hall@email.com", "Mary's Account 1", "maryhall268" },
                    { 18, 9141m, "mary_hall@email.com", "Mary's Account 2", "maryhall268" },
                    { 19, 1522m, "mary_hall@email.com", "Mary's Account 3", "maryhall268" },
                    { 20, 1779m, "barbara_parker@email.com", "Barbara's Account 1", "barbaraparker204" },
                    { 21, 5507m, "barbara_parker@email.com", "Barbara's Account 2", "barbaraparker204" },
                    { 22, 2384m, "barbara_parker@email.com", "Barbara's Account 3", "barbaraparker204" },
                    { 23, 7834m, "barbara_parker@email.com", "Barbara's Account 4", "barbaraparker204" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 88m, "Deposit" },
                    { 2, 1, 510m, "Withdraw" },
                    { 3, 1, 402m, "Withdraw" },
                    { 4, 2, 188m, "Withdraw" },
                    { 5, 2, 944m, "Deposit" },
                    { 6, 2, 773m, "Deposit" },
                    { 7, 2, 100m, "Deposit" },
                    { 8, 2, 152m, "Withdraw" },
                    { 9, 2, 996m, "Withdraw" },
                    { 10, 2, 377m, "Withdraw" },
                    { 11, 2, 438m, "Deposit" },
                    { 12, 2, 67m, "Deposit" },
                    { 13, 2, 395m, "Deposit" },
                    { 14, 3, 126m, "Withdraw" },
                    { 15, 3, 280m, "Deposit" },
                    { 16, 4, 786m, "Deposit" },
                    { 17, 4, 218m, "Withdraw" },
                    { 18, 4, 646m, "Withdraw" },
                    { 19, 4, 276m, "Withdraw" },
                    { 20, 4, 798m, "Deposit" },
                    { 21, 4, 49m, "Withdraw" },
                    { 22, 4, 476m, "Withdraw" },
                    { 23, 4, 144m, "Withdraw" },
                    { 24, 4, 915m, "Deposit" },
                    { 25, 4, 936m, "Deposit" },
                    { 26, 4, 524m, "Deposit" },
                    { 27, 5, 640m, "Withdraw" },
                    { 28, 5, 959m, "Withdraw" },
                    { 29, 5, 208m, "Deposit" },
                    { 30, 5, 284m, "Deposit" },
                    { 31, 5, 362m, "Withdraw" },
                    { 32, 5, 893m, "Withdraw" },
                    { 33, 5, 463m, "Withdraw" },
                    { 34, 5, 447m, "Withdraw" },
                    { 35, 5, 476m, "Withdraw" },
                    { 36, 5, 958m, "Withdraw" },
                    { 37, 5, 474m, "Withdraw" },
                    { 38, 6, 418m, "Withdraw" },
                    { 39, 6, 676m, "Withdraw" },
                    { 40, 6, 555m, "Deposit" },
                    { 41, 6, 513m, "Deposit" },
                    { 42, 6, 220m, "Withdraw" },
                    { 43, 6, 283m, "Deposit" },
                    { 44, 6, 729m, "Withdraw" },
                    { 45, 7, 193m, "Deposit" },
                    { 46, 7, 371m, "Withdraw" },
                    { 47, 7, 43m, "Deposit" },
                    { 48, 7, 854m, "Deposit" },
                    { 49, 7, 523m, "Deposit" },
                    { 50, 7, 774m, "Deposit" },
                    { 51, 7, 632m, "Deposit" },
                    { 52, 7, 594m, "Deposit" },
                    { 53, 7, 562m, "Withdraw" },
                    { 54, 7, 790m, "Deposit" },
                    { 55, 7, 547m, "Withdraw" },
                    { 56, 7, 576m, "Deposit" },
                    { 57, 8, 968m, "Withdraw" },
                    { 58, 8, 737m, "Withdraw" },
                    { 59, 8, 223m, "Withdraw" },
                    { 60, 8, 301m, "Deposit" },
                    { 61, 8, 430m, "Withdraw" },
                    { 62, 9, 415m, "Deposit" },
                    { 63, 10, 414m, "Withdraw" },
                    { 64, 10, 585m, "Deposit" },
                    { 65, 10, 343m, "Deposit" },
                    { 66, 10, 949m, "Withdraw" },
                    { 67, 10, 802m, "Deposit" },
                    { 68, 11, 620m, "Withdraw" },
                    { 69, 11, 501m, "Deposit" },
                    { 70, 11, 400m, "Deposit" },
                    { 71, 11, 724m, "Deposit" },
                    { 72, 11, 866m, "Withdraw" },
                    { 73, 12, 665m, "Deposit" },
                    { 74, 12, 388m, "Withdraw" },
                    { 75, 12, 782m, "Deposit" },
                    { 76, 12, 432m, "Deposit" },
                    { 77, 12, 927m, "Withdraw" },
                    { 78, 12, 951m, "Withdraw" },
                    { 79, 12, 582m, "Withdraw" },
                    { 80, 12, 97m, "Deposit" },
                    { 81, 12, 537m, "Deposit" },
                    { 82, 13, 487m, "Deposit" },
                    { 83, 13, 734m, "Deposit" },
                    { 84, 13, 32m, "Withdraw" },
                    { 85, 13, 301m, "Deposit" },
                    { 86, 13, 334m, "Deposit" },
                    { 87, 13, 416m, "Withdraw" },
                    { 88, 13, 597m, "Deposit" },
                    { 89, 13, 128m, "Deposit" },
                    { 90, 14, 181m, "Deposit" },
                    { 91, 14, 203m, "Withdraw" },
                    { 92, 14, 755m, "Withdraw" },
                    { 93, 14, 489m, "Withdraw" },
                    { 94, 14, 210m, "Deposit" },
                    { 95, 14, 411m, "Withdraw" },
                    { 96, 14, 663m, "Withdraw" },
                    { 97, 14, 959m, "Deposit" },
                    { 98, 14, 877m, "Withdraw" },
                    { 99, 14, 646m, "Withdraw" },
                    { 100, 15, 662m, "Withdraw" },
                    { 101, 15, 678m, "Deposit" },
                    { 102, 16, 922m, "Withdraw" },
                    { 103, 17, 543m, "Withdraw" },
                    { 104, 18, 338m, "Withdraw" },
                    { 105, 19, 844m, "Deposit" },
                    { 106, 19, 487m, "Withdraw" },
                    { 107, 19, 281m, "Deposit" },
                    { 108, 20, 600m, "Deposit" },
                    { 109, 20, 346m, "Withdraw" },
                    { 110, 20, 991m, "Deposit" },
                    { 111, 20, 910m, "Withdraw" },
                    { 112, 20, 639m, "Deposit" },
                    { 113, 20, 843m, "Deposit" },
                    { 114, 20, 958m, "Deposit" },
                    { 115, 20, 752m, "Deposit" },
                    { 116, 20, 747m, "Deposit" },
                    { 117, 21, 175m, "Withdraw" },
                    { 118, 21, 357m, "Withdraw" },
                    { 119, 21, 568m, "Withdraw" },
                    { 120, 22, 243m, "Deposit" },
                    { 121, 22, 989m, "Deposit" },
                    { 122, 23, 10m, "Withdraw" },
                    { 123, 23, 760m, "Withdraw" },
                    { 124, 23, 777m, "Withdraw" }
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
