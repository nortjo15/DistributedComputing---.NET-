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
                    { "jennifergomez600", "169 Gomez Street, City 7", "jennifer_gomez@email.com", "pass7064", "0498755253", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "josephevans238", "5 Evans Street, City 4", "joseph_evans@email.com", "pass2605", "0469833335", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "lindagreen180", "22 Green Street, City 4", "linda_green@email.com", "pass2596", "0432569628", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "michaelhall325", "57 Hall Street, City 8", "michael_hall@email.com", "pass8756", "0472765138", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "richardreyes172", "165 Reyes Street, City 8", "richard_reyes@email.com", "pass5413", "0434803145", "C:\\Users\\joshu\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Username" },
                values: new object[,]
                {
                    { 1, 1279m, "linda_green@email.com", "lindagreen180" },
                    { 2, 9602m, "linda_green@email.com", "lindagreen180" },
                    { 3, 7485m, "linda_green@email.com", "lindagreen180" },
                    { 4, 3544m, "jennifer_gomez@email.com", "jennifergomez600" },
                    { 5, 9229m, "jennifer_gomez@email.com", "jennifergomez600" },
                    { 6, 6070m, "jennifer_gomez@email.com", "jennifergomez600" },
                    { 7, 6897m, "joseph_evans@email.com", "josephevans238" },
                    { 8, 132m, "joseph_evans@email.com", "josephevans238" },
                    { 9, 8517m, "joseph_evans@email.com", "josephevans238" },
                    { 10, 4357m, "michael_hall@email.com", "michaelhall325" },
                    { 11, 8198m, "michael_hall@email.com", "michaelhall325" },
                    { 12, 5285m, "michael_hall@email.com", "michaelhall325" },
                    { 13, 4144m, "richard_reyes@email.com", "richardreyes172" },
                    { 14, 5442m, "richard_reyes@email.com", "richardreyes172" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 850m, "Withdraw" },
                    { 2, 1, 611m, "Deposit" },
                    { 3, 1, 961m, "Deposit" },
                    { 4, 1, 402m, "Withdraw" },
                    { 5, 1, 895m, "Withdraw" },
                    { 6, 1, 98m, "Withdraw" },
                    { 7, 1, 479m, "Deposit" },
                    { 8, 2, 587m, "Deposit" },
                    { 9, 2, 20m, "Deposit" },
                    { 10, 2, 425m, "Withdraw" },
                    { 11, 2, 712m, "Deposit" },
                    { 12, 2, 658m, "Deposit" },
                    { 13, 3, 442m, "Withdraw" },
                    { 14, 3, 984m, "Withdraw" },
                    { 15, 3, 111m, "Deposit" },
                    { 16, 4, 442m, "Deposit" },
                    { 17, 4, 97m, "Withdraw" },
                    { 18, 4, 902m, "Withdraw" },
                    { 19, 4, 488m, "Deposit" },
                    { 20, 4, 207m, "Withdraw" },
                    { 21, 4, 365m, "Deposit" },
                    { 22, 4, 157m, "Deposit" },
                    { 23, 5, 547m, "Deposit" },
                    { 24, 5, 800m, "Withdraw" },
                    { 25, 5, 606m, "Deposit" },
                    { 26, 6, 266m, "Deposit" },
                    { 27, 6, 68m, "Withdraw" },
                    { 28, 6, 643m, "Deposit" },
                    { 29, 6, 865m, "Deposit" },
                    { 30, 6, 376m, "Withdraw" },
                    { 31, 6, 973m, "Deposit" },
                    { 32, 6, 590m, "Withdraw" },
                    { 33, 6, 422m, "Deposit" },
                    { 34, 6, 453m, "Withdraw" },
                    { 35, 7, 429m, "Deposit" },
                    { 36, 7, 379m, "Withdraw" },
                    { 37, 7, 107m, "Withdraw" },
                    { 38, 7, 708m, "Withdraw" },
                    { 39, 7, 962m, "Deposit" },
                    { 40, 7, 874m, "Deposit" },
                    { 41, 7, 961m, "Withdraw" },
                    { 42, 7, 807m, "Deposit" },
                    { 43, 8, 26m, "Deposit" },
                    { 44, 8, 352m, "Deposit" },
                    { 45, 8, 112m, "Deposit" },
                    { 46, 9, 436m, "Deposit" },
                    { 47, 9, 818m, "Withdraw" },
                    { 48, 9, 188m, "Withdraw" },
                    { 49, 10, 722m, "Deposit" },
                    { 50, 10, 258m, "Deposit" },
                    { 51, 10, 307m, "Withdraw" },
                    { 52, 10, 132m, "Withdraw" },
                    { 53, 10, 548m, "Withdraw" },
                    { 54, 11, 378m, "Withdraw" },
                    { 55, 11, 308m, "Withdraw" },
                    { 56, 11, 541m, "Withdraw" },
                    { 57, 11, 978m, "Withdraw" },
                    { 58, 11, 993m, "Withdraw" },
                    { 59, 12, 955m, "Withdraw" },
                    { 60, 12, 62m, "Withdraw" },
                    { 61, 12, 539m, "Deposit" },
                    { 62, 13, 652m, "Deposit" },
                    { 63, 13, 168m, "Withdraw" },
                    { 64, 13, 939m, "Withdraw" },
                    { 65, 13, 360m, "Deposit" },
                    { 66, 13, 966m, "Withdraw" },
                    { 67, 14, 65m, "Withdraw" }
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
