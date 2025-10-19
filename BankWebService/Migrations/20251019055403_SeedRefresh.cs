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
                    { "BankAdmin2", "Head Office", "admin2@bank.local", "ChangeMe!123", "+0000000000", "/images/admin.png" },
                    { "BankAdmin3", "Head Office", "admin3@bank.local", "ChangeMe!123", "+0000000000", "/images/admin.png" },
                    { "jamesgreen536", "136 Green Street, City 7", "james_green@email.com", "pass2583", "0433541641", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "jessicaedwards410", "117 Edwards Street, City 4", "jessica_edwards@email.com", "pass5811", "0445853982", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "karengreen872", "199 Green Street, City 4", "karen_green@email.com", "pass8499", "0463737057", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "lindaflores538", "145 Flores Street, City 9", "linda_flores@email.com", "pass7874", "0449920721", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "lindagomez831", "131 Gomez Street, City 3", "linda_gomez@email.com", "pass5206", "0438736231", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\5.jpg" },
                    { "lindareyes520", "86 Reyes Street, City 9", "linda_reyes@email.com", "pass8585", "0430753479", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "marygreen176", "197 Green Street, City 1", "mary_green@email.com", "pass5490", "0496277608", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Name", "Username" },
                values: new object[,]
                {
                    { 1, 9256m, "linda_reyes@email.com", "Linda's Account 1", "lindareyes520" },
                    { 2, 536m, "linda_reyes@email.com", "Linda's Account 2", "lindareyes520" },
                    { 3, 488m, "linda_reyes@email.com", "Linda's Account 3", "lindareyes520" },
                    { 4, 5411m, "linda_reyes@email.com", "Linda's Account 4", "lindareyes520" },
                    { 5, 8985m, "james_green@email.com", "James's Account 1", "jamesgreen536" },
                    { 6, 9309m, "james_green@email.com", "James's Account 2", "jamesgreen536" },
                    { 7, 3626m, "james_green@email.com", "James's Account 3", "jamesgreen536" },
                    { 8, 6023m, "james_green@email.com", "James's Account 4", "jamesgreen536" },
                    { 9, 2351m, "james_green@email.com", "James's Account 5", "jamesgreen536" },
                    { 10, 5975m, "mary_green@email.com", "Mary's Account 1", "marygreen176" },
                    { 11, 3657m, "mary_green@email.com", "Mary's Account 2", "marygreen176" },
                    { 12, 6397m, "mary_green@email.com", "Mary's Account 3", "marygreen176" },
                    { 13, 2681m, "mary_green@email.com", "Mary's Account 4", "marygreen176" },
                    { 14, 4845m, "jessica_edwards@email.com", "Jessica's Account 1", "jessicaedwards410" },
                    { 15, 8818m, "jessica_edwards@email.com", "Jessica's Account 2", "jessicaedwards410" },
                    { 16, 8038m, "jessica_edwards@email.com", "Jessica's Account 3", "jessicaedwards410" },
                    { 17, 7951m, "jessica_edwards@email.com", "Jessica's Account 4", "jessicaedwards410" },
                    { 18, 5173m, "jessica_edwards@email.com", "Jessica's Account 5", "jessicaedwards410" },
                    { 19, 3138m, "karen_green@email.com", "Karen's Account 1", "karengreen872" },
                    { 20, 2496m, "karen_green@email.com", "Karen's Account 2", "karengreen872" },
                    { 21, 4309m, "karen_green@email.com", "Karen's Account 3", "karengreen872" },
                    { 22, 9724m, "karen_green@email.com", "Karen's Account 4", "karengreen872" },
                    { 23, 1590m, "karen_green@email.com", "Karen's Account 5", "karengreen872" },
                    { 24, 8945m, "linda_flores@email.com", "Linda's Account 1", "lindaflores538" },
                    { 25, 8543m, "linda_gomez@email.com", "Linda's Account 1", "lindagomez831" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 426m, "Withdraw" },
                    { 2, 1, 303m, "Deposit" },
                    { 3, 1, 570m, "Withdraw" },
                    { 4, 1, 298m, "Withdraw" },
                    { 5, 1, 506m, "Deposit" },
                    { 6, 1, 123m, "Deposit" },
                    { 7, 1, 342m, "Withdraw" },
                    { 8, 1, 60m, "Withdraw" },
                    { 9, 1, 818m, "Withdraw" },
                    { 10, 2, 462m, "Withdraw" },
                    { 11, 2, 669m, "Deposit" },
                    { 12, 3, 969m, "Withdraw" },
                    { 13, 3, 36m, "Deposit" },
                    { 14, 3, 175m, "Deposit" },
                    { 15, 3, 154m, "Withdraw" },
                    { 16, 4, 113m, "Deposit" },
                    { 17, 4, 459m, "Withdraw" },
                    { 18, 5, 173m, "Withdraw" },
                    { 19, 5, 522m, "Withdraw" },
                    { 20, 5, 720m, "Withdraw" },
                    { 21, 5, 363m, "Deposit" },
                    { 22, 5, 957m, "Withdraw" },
                    { 23, 5, 721m, "Withdraw" },
                    { 24, 6, 659m, "Withdraw" },
                    { 25, 6, 950m, "Withdraw" },
                    { 26, 6, 166m, "Withdraw" },
                    { 27, 6, 543m, "Withdraw" },
                    { 28, 6, 760m, "Deposit" },
                    { 29, 6, 663m, "Withdraw" },
                    { 30, 6, 507m, "Deposit" },
                    { 31, 7, 24m, "Withdraw" },
                    { 32, 7, 439m, "Deposit" },
                    { 33, 7, 301m, "Deposit" },
                    { 34, 7, 379m, "Withdraw" },
                    { 35, 7, 138m, "Withdraw" },
                    { 36, 7, 730m, "Withdraw" },
                    { 37, 7, 775m, "Withdraw" },
                    { 38, 7, 920m, "Withdraw" },
                    { 39, 7, 126m, "Deposit" },
                    { 40, 7, 337m, "Deposit" },
                    { 41, 7, 794m, "Withdraw" },
                    { 42, 7, 176m, "Deposit" },
                    { 43, 8, 285m, "Deposit" },
                    { 44, 8, 858m, "Deposit" },
                    { 45, 8, 725m, "Deposit" },
                    { 46, 8, 213m, "Withdraw" },
                    { 47, 8, 618m, "Deposit" },
                    { 48, 8, 735m, "Deposit" },
                    { 49, 8, 111m, "Withdraw" },
                    { 50, 8, 677m, "Withdraw" },
                    { 51, 9, 604m, "Withdraw" },
                    { 52, 9, 134m, "Withdraw" },
                    { 53, 9, 434m, "Deposit" },
                    { 54, 9, 607m, "Deposit" },
                    { 55, 10, 320m, "Withdraw" },
                    { 56, 10, 83m, "Withdraw" },
                    { 57, 10, 754m, "Deposit" },
                    { 58, 10, 12m, "Deposit" },
                    { 59, 10, 154m, "Withdraw" },
                    { 60, 11, 397m, "Withdraw" },
                    { 61, 11, 294m, "Deposit" },
                    { 62, 11, 26m, "Withdraw" },
                    { 63, 11, 316m, "Deposit" },
                    { 64, 11, 772m, "Withdraw" },
                    { 65, 11, 868m, "Withdraw" },
                    { 66, 11, 621m, "Withdraw" },
                    { 67, 11, 301m, "Withdraw" },
                    { 68, 11, 846m, "Deposit" },
                    { 69, 11, 978m, "Withdraw" },
                    { 70, 11, 680m, "Deposit" },
                    { 71, 11, 12m, "Withdraw" },
                    { 72, 12, 538m, "Withdraw" },
                    { 73, 12, 97m, "Withdraw" },
                    { 74, 12, 833m, "Deposit" },
                    { 75, 12, 75m, "Withdraw" },
                    { 76, 12, 168m, "Withdraw" },
                    { 77, 12, 862m, "Withdraw" },
                    { 78, 12, 950m, "Withdraw" },
                    { 79, 12, 22m, "Deposit" },
                    { 80, 12, 234m, "Deposit" },
                    { 81, 12, 54m, "Withdraw" },
                    { 82, 12, 911m, "Withdraw" },
                    { 83, 12, 199m, "Withdraw" },
                    { 84, 13, 478m, "Deposit" },
                    { 85, 13, 965m, "Deposit" },
                    { 86, 13, 513m, "Deposit" },
                    { 87, 14, 598m, "Withdraw" },
                    { 88, 14, 813m, "Withdraw" },
                    { 89, 14, 334m, "Deposit" },
                    { 90, 14, 445m, "Deposit" },
                    { 91, 14, 943m, "Deposit" },
                    { 92, 14, 353m, "Deposit" },
                    { 93, 14, 606m, "Deposit" },
                    { 94, 15, 923m, "Withdraw" },
                    { 95, 15, 465m, "Deposit" },
                    { 96, 15, 842m, "Withdraw" },
                    { 97, 15, 482m, "Deposit" },
                    { 98, 15, 112m, "Deposit" },
                    { 99, 15, 679m, "Withdraw" },
                    { 100, 15, 270m, "Deposit" },
                    { 101, 15, 568m, "Deposit" },
                    { 102, 15, 323m, "Withdraw" },
                    { 103, 15, 369m, "Withdraw" },
                    { 104, 15, 914m, "Withdraw" },
                    { 105, 15, 485m, "Deposit" },
                    { 106, 16, 217m, "Withdraw" },
                    { 107, 17, 305m, "Withdraw" },
                    { 108, 17, 770m, "Withdraw" },
                    { 109, 17, 152m, "Deposit" },
                    { 110, 17, 281m, "Deposit" },
                    { 111, 18, 84m, "Withdraw" },
                    { 112, 18, 984m, "Deposit" },
                    { 113, 18, 722m, "Withdraw" },
                    { 114, 18, 247m, "Deposit" },
                    { 115, 18, 277m, "Withdraw" },
                    { 116, 18, 57m, "Withdraw" },
                    { 117, 18, 184m, "Withdraw" },
                    { 118, 18, 257m, "Withdraw" },
                    { 119, 19, 597m, "Withdraw" },
                    { 120, 19, 193m, "Withdraw" },
                    { 121, 19, 209m, "Withdraw" },
                    { 122, 19, 49m, "Withdraw" },
                    { 123, 19, 724m, "Withdraw" },
                    { 124, 19, 703m, "Withdraw" },
                    { 125, 19, 430m, "Withdraw" },
                    { 126, 19, 258m, "Withdraw" },
                    { 127, 20, 294m, "Withdraw" },
                    { 128, 20, 791m, "Withdraw" },
                    { 129, 20, 597m, "Withdraw" },
                    { 130, 20, 337m, "Withdraw" },
                    { 131, 20, 886m, "Deposit" },
                    { 132, 20, 39m, "Deposit" },
                    { 133, 20, 224m, "Deposit" },
                    { 134, 20, 195m, "Deposit" },
                    { 135, 20, 453m, "Withdraw" },
                    { 136, 20, 158m, "Deposit" },
                    { 137, 20, 211m, "Withdraw" },
                    { 138, 20, 232m, "Deposit" },
                    { 139, 21, 877m, "Deposit" },
                    { 140, 22, 165m, "Withdraw" },
                    { 141, 22, 266m, "Deposit" },
                    { 142, 22, 380m, "Withdraw" },
                    { 143, 22, 37m, "Withdraw" },
                    { 144, 22, 322m, "Deposit" },
                    { 145, 22, 757m, "Withdraw" },
                    { 146, 22, 245m, "Deposit" },
                    { 147, 22, 860m, "Deposit" },
                    { 148, 22, 399m, "Withdraw" },
                    { 149, 22, 505m, "Deposit" },
                    { 150, 22, 627m, "Withdraw" },
                    { 151, 22, 134m, "Deposit" },
                    { 152, 23, 725m, "Deposit" },
                    { 153, 23, 466m, "Deposit" },
                    { 154, 23, 743m, "Withdraw" },
                    { 155, 23, 562m, "Deposit" },
                    { 156, 23, 936m, "Deposit" },
                    { 157, 23, 812m, "Withdraw" },
                    { 158, 23, 86m, "Withdraw" },
                    { 159, 23, 920m, "Withdraw" },
                    { 160, 23, 676m, "Deposit" },
                    { 161, 24, 602m, "Withdraw" },
                    { 162, 25, 175m, "Withdraw" },
                    { 163, 25, 567m, "Withdraw" },
                    { 164, 25, 70m, "Withdraw" }
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
