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
                    { "jamescollins161", "2 Collins Street, City 3", "james_collins@email.com", "pass8675", "0479061052", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "jennifermitchell861", "14 Mitchell Street, City 5", "jennifer_mitchell@email.com", "pass5866", "0455686560", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "lindacarter752", "134 Carter Street, City 3", "linda_carter@email.com", "pass1429", "0492784652", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "lindareyes715", "155 Reyes Street, City 9", "linda_reyes@email.com", "pass1831", "0430220024", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "williamparker667", "42 Parker Street, City 7", "william_parker@email.com", "pass9784", "0493662132", "C:\\Users\\jjse5\\Source\\Repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Username" },
                values: new object[,]
                {
                    { 1, 1918m, "james_collins@email.com", "jamescollins161" },
                    { 2, 1659m, "james_collins@email.com", "jamescollins161" },
                    { 3, 6578m, "james_collins@email.com", "jamescollins161" },
                    { 4, 3510m, "linda_reyes@email.com", "lindareyes715" },
                    { 5, 2879m, "linda_reyes@email.com", "lindareyes715" },
                    { 6, 387m, "linda_reyes@email.com", "lindareyes715" },
                    { 7, 7595m, "jennifer_mitchell@email.com", "jennifermitchell861" },
                    { 8, 4974m, "jennifer_mitchell@email.com", "jennifermitchell861" },
                    { 9, 4191m, "jennifer_mitchell@email.com", "jennifermitchell861" },
                    { 10, 2569m, "william_parker@email.com", "williamparker667" },
                    { 11, 3763m, "william_parker@email.com", "williamparker667" },
                    { 12, 7006m, "linda_carter@email.com", "lindacarter752" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 284m, "Withdraw" },
                    { 2, 1, 19m, "Withdraw" },
                    { 3, 1, 449m, "Withdraw" },
                    { 4, 1, 973m, "Withdraw" },
                    { 5, 1, 977m, "Withdraw" },
                    { 6, 2, 313m, "Withdraw" },
                    { 7, 2, 21m, "Deposit" },
                    { 8, 2, 697m, "Withdraw" },
                    { 9, 2, 781m, "Deposit" },
                    { 10, 2, 812m, "Deposit" },
                    { 11, 2, 175m, "Deposit" },
                    { 12, 2, 545m, "Withdraw" },
                    { 13, 2, 287m, "Withdraw" },
                    { 14, 2, 963m, "Withdraw" },
                    { 15, 3, 709m, "Deposit" },
                    { 16, 3, 667m, "Deposit" },
                    { 17, 3, 337m, "Withdraw" },
                    { 18, 3, 33m, "Deposit" },
                    { 19, 3, 650m, "Deposit" },
                    { 20, 3, 471m, "Withdraw" },
                    { 21, 4, 996m, "Withdraw" },
                    { 22, 4, 647m, "Withdraw" },
                    { 23, 4, 902m, "Deposit" },
                    { 24, 4, 837m, "Deposit" },
                    { 25, 4, 165m, "Withdraw" },
                    { 26, 5, 244m, "Deposit" },
                    { 27, 5, 404m, "Deposit" },
                    { 28, 5, 380m, "Withdraw" },
                    { 29, 5, 846m, "Withdraw" },
                    { 30, 5, 12m, "Withdraw" },
                    { 31, 5, 52m, "Withdraw" },
                    { 32, 5, 387m, "Deposit" },
                    { 33, 5, 717m, "Withdraw" },
                    { 34, 5, 994m, "Withdraw" },
                    { 35, 5, 470m, "Deposit" },
                    { 36, 6, 272m, "Withdraw" },
                    { 37, 6, 996m, "Withdraw" },
                    { 38, 6, 834m, "Deposit" },
                    { 39, 6, 175m, "Deposit" },
                    { 40, 6, 342m, "Deposit" },
                    { 41, 6, 80m, "Deposit" },
                    { 42, 6, 170m, "Withdraw" },
                    { 43, 7, 859m, "Deposit" },
                    { 44, 7, 487m, "Withdraw" },
                    { 45, 7, 139m, "Withdraw" },
                    { 46, 7, 29m, "Deposit" },
                    { 47, 7, 804m, "Withdraw" },
                    { 48, 7, 934m, "Withdraw" },
                    { 49, 7, 622m, "Deposit" },
                    { 50, 7, 307m, "Withdraw" },
                    { 51, 8, 644m, "Deposit" },
                    { 52, 8, 930m, "Deposit" },
                    { 53, 9, 183m, "Deposit" },
                    { 54, 9, 682m, "Deposit" },
                    { 55, 9, 981m, "Deposit" },
                    { 56, 9, 323m, "Deposit" },
                    { 57, 9, 271m, "Withdraw" },
                    { 58, 9, 804m, "Deposit" },
                    { 59, 9, 834m, "Deposit" },
                    { 60, 9, 200m, "Withdraw" },
                    { 61, 9, 516m, "Deposit" },
                    { 62, 9, 551m, "Withdraw" },
                    { 63, 10, 573m, "Withdraw" },
                    { 64, 10, 159m, "Withdraw" },
                    { 65, 10, 21m, "Withdraw" },
                    { 66, 10, 27m, "Deposit" },
                    { 67, 10, 722m, "Deposit" },
                    { 68, 10, 705m, "Deposit" },
                    { 69, 10, 35m, "Deposit" },
                    { 70, 10, 222m, "Deposit" },
                    { 71, 10, 755m, "Deposit" },
                    { 72, 11, 691m, "Withdraw" },
                    { 73, 11, 924m, "Withdraw" },
                    { 74, 11, 580m, "Withdraw" },
                    { 75, 11, 891m, "Deposit" },
                    { 76, 12, 128m, "Deposit" },
                    { 77, 12, 539m, "Deposit" },
                    { 78, 12, 593m, "Withdraw" },
                    { 79, 12, 469m, "Deposit" },
                    { 80, 12, 267m, "Deposit" },
                    { 81, 12, 588m, "Withdraw" },
                    { 82, 12, 279m, "Withdraw" }
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
