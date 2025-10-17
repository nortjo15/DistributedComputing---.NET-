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
                    { "barbaragreen251", "96 Green Street, City 8", "barbara_green@email.com", "pass7672", "0461899010", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "christophermitchell236", "72 Mitchell Street, City 1", "christopher_mitchell@email.com", "pass2229", "0462995421", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "elizabethevans368", "106 Evans Street, City 8", "elizabeth_evans@email.com", "pass9791", "0495949095", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "jamesflores126", "43 Flores Street, City 1", "james_flores@email.com", "pass1713", "0444380740", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "lindacarter117", "72 Carter Street, City 1", "linda_carter@email.com", "pass8561", "0478173708", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "richardparker167", "79 Parker Street, City 8", "richard_parker@email.com", "pass2200", "0427307348", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "susancarter357", "96 Carter Street, City 1", "susan_carter@email.com", "pass7469", "0452067754", "C:\\Users\\jjse5\\source\\repos\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Username" },
                values: new object[,]
                {
                    { 1, 1268m, "james_flores@email.com", "jamesflores126" },
                    { 2, 2955m, "james_flores@email.com", "jamesflores126" },
                    { 3, 7626m, "james_flores@email.com", "jamesflores126" },
                    { 4, 8586m, "james_flores@email.com", "jamesflores126" },
                    { 5, 2282m, "james_flores@email.com", "jamesflores126" },
                    { 6, 8313m, "james_flores@email.com", "jamesflores126" },
                    { 7, 3545m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 8, 2867m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 9, 1922m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 10, 5569m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 11, 6332m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 12, 2942m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 13, 8952m, "christopher_mitchell@email.com", "christophermitchell236" },
                    { 14, 1205m, "susan_carter@email.com", "susancarter357" },
                    { 15, 3906m, "susan_carter@email.com", "susancarter357" },
                    { 16, 5831m, "susan_carter@email.com", "susancarter357" },
                    { 17, 6566m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 18, 9461m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 19, 3989m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 20, 8823m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 21, 5161m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 22, 419m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 23, 5985m, "elizabeth_evans@email.com", "elizabethevans368" },
                    { 24, 4281m, "barbara_green@email.com", "barbaragreen251" },
                    { 25, 8194m, "barbara_green@email.com", "barbaragreen251" },
                    { 26, 9657m, "barbara_green@email.com", "barbaragreen251" },
                    { 27, 8974m, "linda_carter@email.com", "lindacarter117" },
                    { 28, 7123m, "linda_carter@email.com", "lindacarter117" },
                    { 29, 7115m, "linda_carter@email.com", "lindacarter117" },
                    { 30, 9166m, "linda_carter@email.com", "lindacarter117" },
                    { 31, 184m, "linda_carter@email.com", "lindacarter117" },
                    { 32, 4528m, "linda_carter@email.com", "lindacarter117" },
                    { 33, 7597m, "linda_carter@email.com", "lindacarter117" },
                    { 34, 1563m, "linda_carter@email.com", "lindacarter117" },
                    { 35, 2618m, "richard_parker@email.com", "richardparker167" },
                    { 36, 4588m, "richard_parker@email.com", "richardparker167" },
                    { 37, 9321m, "richard_parker@email.com", "richardparker167" },
                    { 38, 2486m, "richard_parker@email.com", "richardparker167" },
                    { 39, 6264m, "richard_parker@email.com", "richardparker167" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 490m, "Deposit" },
                    { 2, 2, 499m, "Withdraw" },
                    { 3, 2, 539m, "Withdraw" },
                    { 4, 2, 101m, "Withdraw" },
                    { 5, 2, 272m, "Withdraw" },
                    { 6, 2, 291m, "Deposit" },
                    { 7, 2, 15m, "Deposit" },
                    { 8, 3, 221m, "Deposit" },
                    { 9, 3, 468m, "Withdraw" },
                    { 10, 3, 450m, "Withdraw" },
                    { 11, 3, 588m, "Withdraw" },
                    { 12, 3, 521m, "Withdraw" },
                    { 13, 3, 708m, "Deposit" },
                    { 14, 4, 670m, "Withdraw" },
                    { 15, 4, 923m, "Withdraw" },
                    { 16, 4, 799m, "Deposit" },
                    { 17, 4, 827m, "Withdraw" },
                    { 18, 4, 939m, "Deposit" },
                    { 19, 4, 89m, "Deposit" },
                    { 20, 4, 16m, "Withdraw" },
                    { 21, 4, 922m, "Deposit" },
                    { 22, 4, 717m, "Deposit" },
                    { 23, 5, 701m, "Withdraw" },
                    { 24, 5, 634m, "Deposit" },
                    { 25, 5, 661m, "Withdraw" },
                    { 26, 5, 698m, "Deposit" },
                    { 27, 5, 247m, "Deposit" },
                    { 28, 5, 924m, "Withdraw" },
                    { 29, 6, 318m, "Deposit" },
                    { 30, 6, 473m, "Deposit" },
                    { 31, 6, 282m, "Withdraw" },
                    { 32, 6, 990m, "Withdraw" },
                    { 33, 6, 213m, "Withdraw" },
                    { 34, 6, 880m, "Deposit" },
                    { 35, 7, 520m, "Deposit" },
                    { 36, 7, 702m, "Deposit" },
                    { 37, 7, 384m, "Withdraw" },
                    { 38, 7, 493m, "Withdraw" },
                    { 39, 7, 841m, "Withdraw" },
                    { 40, 8, 144m, "Withdraw" },
                    { 41, 8, 133m, "Withdraw" },
                    { 42, 8, 766m, "Deposit" },
                    { 43, 8, 102m, "Deposit" },
                    { 44, 8, 364m, "Deposit" },
                    { 45, 8, 663m, "Withdraw" },
                    { 46, 9, 149m, "Deposit" },
                    { 47, 9, 542m, "Withdraw" },
                    { 48, 9, 24m, "Deposit" },
                    { 49, 10, 548m, "Withdraw" },
                    { 50, 10, 929m, "Deposit" },
                    { 51, 10, 661m, "Withdraw" },
                    { 52, 10, 407m, "Withdraw" },
                    { 53, 10, 76m, "Deposit" },
                    { 54, 10, 533m, "Deposit" },
                    { 55, 10, 695m, "Deposit" },
                    { 56, 10, 241m, "Withdraw" },
                    { 57, 10, 63m, "Withdraw" },
                    { 58, 11, 614m, "Deposit" },
                    { 59, 11, 972m, "Withdraw" },
                    { 60, 11, 604m, "Deposit" },
                    { 61, 12, 210m, "Withdraw" },
                    { 62, 13, 940m, "Deposit" },
                    { 63, 14, 433m, "Deposit" },
                    { 64, 14, 224m, "Deposit" },
                    { 65, 14, 639m, "Withdraw" },
                    { 66, 14, 185m, "Deposit" },
                    { 67, 14, 723m, "Withdraw" },
                    { 68, 14, 872m, "Withdraw" },
                    { 69, 14, 949m, "Withdraw" },
                    { 70, 14, 313m, "Deposit" },
                    { 71, 15, 647m, "Withdraw" },
                    { 72, 15, 168m, "Withdraw" },
                    { 73, 16, 225m, "Withdraw" },
                    { 74, 16, 747m, "Withdraw" },
                    { 75, 16, 259m, "Deposit" },
                    { 76, 16, 397m, "Deposit" },
                    { 77, 16, 65m, "Withdraw" },
                    { 78, 16, 973m, "Deposit" },
                    { 79, 17, 809m, "Withdraw" },
                    { 80, 17, 405m, "Withdraw" },
                    { 81, 17, 810m, "Withdraw" },
                    { 82, 17, 874m, "Withdraw" },
                    { 83, 17, 22m, "Withdraw" },
                    { 84, 17, 863m, "Withdraw" },
                    { 85, 18, 372m, "Withdraw" },
                    { 86, 18, 168m, "Withdraw" },
                    { 87, 18, 428m, "Withdraw" },
                    { 88, 19, 332m, "Deposit" },
                    { 89, 20, 757m, "Deposit" },
                    { 90, 20, 877m, "Withdraw" },
                    { 91, 20, 976m, "Deposit" },
                    { 92, 20, 64m, "Withdraw" },
                    { 93, 20, 84m, "Deposit" },
                    { 94, 20, 517m, "Deposit" },
                    { 95, 20, 600m, "Deposit" },
                    { 96, 21, 200m, "Deposit" },
                    { 97, 21, 871m, "Deposit" },
                    { 98, 21, 856m, "Withdraw" },
                    { 99, 21, 956m, "Withdraw" },
                    { 100, 22, 716m, "Deposit" },
                    { 101, 22, 892m, "Deposit" },
                    { 102, 22, 494m, "Withdraw" },
                    { 103, 22, 29m, "Deposit" },
                    { 104, 22, 487m, "Withdraw" },
                    { 105, 22, 955m, "Deposit" },
                    { 106, 23, 733m, "Withdraw" },
                    { 107, 23, 118m, "Withdraw" },
                    { 108, 23, 548m, "Deposit" },
                    { 109, 24, 664m, "Withdraw" },
                    { 110, 24, 706m, "Deposit" },
                    { 111, 24, 341m, "Deposit" },
                    { 112, 24, 547m, "Deposit" },
                    { 113, 24, 337m, "Deposit" },
                    { 114, 24, 262m, "Withdraw" },
                    { 115, 24, 761m, "Withdraw" },
                    { 116, 25, 214m, "Deposit" },
                    { 117, 25, 265m, "Withdraw" },
                    { 118, 25, 689m, "Deposit" },
                    { 119, 26, 460m, "Withdraw" },
                    { 120, 26, 824m, "Withdraw" },
                    { 121, 26, 535m, "Withdraw" },
                    { 122, 27, 815m, "Deposit" },
                    { 123, 27, 306m, "Deposit" },
                    { 124, 28, 235m, "Deposit" },
                    { 125, 28, 449m, "Deposit" },
                    { 126, 28, 638m, "Deposit" },
                    { 127, 28, 969m, "Withdraw" },
                    { 128, 28, 761m, "Deposit" },
                    { 129, 29, 946m, "Deposit" },
                    { 130, 29, 613m, "Deposit" },
                    { 131, 29, 305m, "Deposit" },
                    { 132, 29, 23m, "Deposit" },
                    { 133, 30, 154m, "Withdraw" },
                    { 134, 30, 190m, "Withdraw" },
                    { 135, 30, 935m, "Withdraw" },
                    { 136, 30, 781m, "Withdraw" },
                    { 137, 30, 546m, "Withdraw" },
                    { 138, 31, 943m, "Deposit" },
                    { 139, 31, 800m, "Withdraw" },
                    { 140, 31, 386m, "Withdraw" },
                    { 141, 31, 304m, "Withdraw" },
                    { 142, 31, 633m, "Deposit" },
                    { 143, 31, 518m, "Deposit" },
                    { 144, 31, 233m, "Deposit" },
                    { 145, 32, 848m, "Withdraw" },
                    { 146, 32, 83m, "Deposit" },
                    { 147, 32, 18m, "Withdraw" },
                    { 148, 32, 50m, "Withdraw" },
                    { 149, 32, 388m, "Deposit" },
                    { 150, 32, 352m, "Withdraw" },
                    { 151, 32, 29m, "Deposit" },
                    { 152, 32, 727m, "Deposit" },
                    { 153, 33, 630m, "Withdraw" },
                    { 154, 33, 65m, "Withdraw" },
                    { 155, 33, 458m, "Deposit" },
                    { 156, 33, 290m, "Withdraw" },
                    { 157, 33, 780m, "Deposit" },
                    { 158, 33, 668m, "Deposit" },
                    { 159, 33, 92m, "Withdraw" },
                    { 160, 33, 597m, "Withdraw" },
                    { 161, 33, 959m, "Deposit" },
                    { 162, 34, 286m, "Withdraw" },
                    { 163, 34, 668m, "Deposit" },
                    { 164, 34, 841m, "Deposit" },
                    { 165, 34, 557m, "Deposit" },
                    { 166, 34, 116m, "Deposit" },
                    { 167, 35, 313m, "Withdraw" },
                    { 168, 35, 493m, "Deposit" },
                    { 169, 35, 491m, "Withdraw" },
                    { 170, 35, 776m, "Withdraw" },
                    { 171, 36, 509m, "Deposit" },
                    { 172, 36, 102m, "Deposit" },
                    { 173, 37, 984m, "Deposit" },
                    { 174, 37, 604m, "Deposit" },
                    { 175, 37, 718m, "Deposit" },
                    { 176, 37, 22m, "Withdraw" },
                    { 177, 37, 488m, "Deposit" },
                    { 178, 37, 334m, "Deposit" },
                    { 179, 37, 784m, "Deposit" },
                    { 180, 37, 33m, "Withdraw" },
                    { 181, 37, 404m, "Withdraw" },
                    { 182, 38, 95m, "Withdraw" },
                    { 183, 38, 168m, "Withdraw" },
                    { 184, 38, 736m, "Withdraw" },
                    { 185, 38, 714m, "Deposit" },
                    { 186, 38, 248m, "Deposit" },
                    { 187, 38, 385m, "Withdraw" },
                    { 188, 39, 492m, "Withdraw" },
                    { 189, 39, 36m, "Deposit" }
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
