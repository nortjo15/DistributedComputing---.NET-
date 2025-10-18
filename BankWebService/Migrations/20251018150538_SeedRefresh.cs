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
                    { "elizabethedwards766", "150 Edwards Street, City 1", "elizabeth_edwards@email.com", "pass6785", "0446077880", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "elizabethrivera552", "78 Rivera Street, City 9", "elizabeth_rivera@email.com", "pass5330", "0436463503", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "jennifercruz522", "113 Cruz Street, City 8", "jennifer_cruz@email.com", "pass2375", "0491244836", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" },
                    { "jessicaparker180", "82 Parker Street, City 9", "jessica_parker@email.com", "pass8826", "0457038127", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "michaelreyes839", "9 Reyes Street, City 2", "michael_reyes@email.com", "pass8454", "0457564300", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\2.jpg" },
                    { "MoeDegrasse", "17 Harper Street, City 2", "MoeDegrasse@email.com", "pass12", "0444122322", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\1.jpg" },
                    { "patriciacruz655", "110 Cruz Street, City 6", "patricia_cruz@email.com", "pass3965", "0435489511", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\3.jpg" },
                    { "richarddiaz386", "124 Diaz Street, City 9", "richard_diaz@email.com", "pass3434", "0426641971", "C:\\Sean\\Sean\\Curtin\\2025 Sem 2\\DC\\Assignment 2\\DC_Assignment_2\\resources\\ProfilePictures\\4.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Bank_Accounts",
                columns: new[] { "AccountNumber", "Balance", "Email", "Name", "Username" },
                values: new object[,]
                {
                    { 1, 7781m, "elizabeth_rivera@email.com", "Elizabeth's Account 1", "elizabethrivera552" },
                    { 2, 2355m, "elizabeth_edwards@email.com", "Elizabeth's Account 1", "elizabethedwards766" },
                    { 3, 9163m, "elizabeth_edwards@email.com", "Elizabeth's Account 2", "elizabethedwards766" },
                    { 4, 6405m, "elizabeth_edwards@email.com", "Elizabeth's Account 3", "elizabethedwards766" },
                    { 5, 9788m, "elizabeth_edwards@email.com", "Elizabeth's Account 4", "elizabethedwards766" },
                    { 6, 3359m, "elizabeth_edwards@email.com", "Elizabeth's Account 5", "elizabethedwards766" },
                    { 7, 6700m, "jennifer_cruz@email.com", "Jennifer's Account 1", "jennifercruz522" },
                    { 8, 9177m, "michael_reyes@email.com", "Michael's Account 1", "michaelreyes839" },
                    { 9, 5072m, "michael_reyes@email.com", "Michael's Account 2", "michaelreyes839" },
                    { 10, 7051m, "michael_reyes@email.com", "Michael's Account 3", "michaelreyes839" },
                    { 11, 570m, "jessica_parker@email.com", "Jessica's Account 1", "jessicaparker180" },
                    { 12, 5645m, "jessica_parker@email.com", "Jessica's Account 2", "jessicaparker180" },
                    { 13, 4469m, "richard_diaz@email.com", "Richard's Account 1", "richarddiaz386" },
                    { 14, 6969m, "patricia_cruz@email.com", "Patricia's Account 1", "patriciacruz655" },
                    { 15, 2524m, "patricia_cruz@email.com", "Patricia's Account 2", "patriciacruz655" },
                    { 16, 8682m, "patricia_cruz@email.com", "Patricia's Account 3", "patriciacruz655" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "TransactionId", "AccountNumber", "Amount", "Type" },
                values: new object[,]
                {
                    { 1, 1, 575m, "Deposit" },
                    { 2, 1, 35m, "Deposit" },
                    { 3, 1, 769m, "Deposit" },
                    { 4, 1, 553m, "Withdraw" },
                    { 5, 1, 698m, "Withdraw" },
                    { 6, 1, 522m, "Deposit" },
                    { 7, 1, 945m, "Deposit" },
                    { 8, 2, 27m, "Withdraw" },
                    { 9, 2, 373m, "Deposit" },
                    { 10, 2, 518m, "Withdraw" },
                    { 11, 2, 951m, "Deposit" },
                    { 12, 2, 75m, "Deposit" },
                    { 13, 2, 260m, "Deposit" },
                    { 14, 2, 274m, "Deposit" },
                    { 15, 2, 485m, "Deposit" },
                    { 16, 2, 811m, "Deposit" },
                    { 17, 2, 400m, "Withdraw" },
                    { 18, 2, 325m, "Deposit" },
                    { 19, 3, 723m, "Deposit" },
                    { 20, 3, 295m, "Withdraw" },
                    { 21, 3, 812m, "Withdraw" },
                    { 22, 3, 721m, "Deposit" },
                    { 23, 3, 58m, "Deposit" },
                    { 24, 3, 672m, "Deposit" },
                    { 25, 3, 389m, "Withdraw" },
                    { 26, 3, 423m, "Withdraw" },
                    { 27, 3, 904m, "Withdraw" },
                    { 28, 3, 312m, "Deposit" },
                    { 29, 4, 190m, "Withdraw" },
                    { 30, 4, 802m, "Deposit" },
                    { 31, 4, 117m, "Deposit" },
                    { 32, 4, 331m, "Withdraw" },
                    { 33, 4, 102m, "Deposit" },
                    { 34, 4, 25m, "Withdraw" },
                    { 35, 4, 501m, "Deposit" },
                    { 36, 4, 270m, "Withdraw" },
                    { 37, 4, 846m, "Deposit" },
                    { 38, 4, 514m, "Withdraw" },
                    { 39, 4, 65m, "Deposit" },
                    { 40, 5, 272m, "Deposit" },
                    { 41, 5, 607m, "Deposit" },
                    { 42, 5, 496m, "Withdraw" },
                    { 43, 5, 60m, "Deposit" },
                    { 44, 5, 196m, "Withdraw" },
                    { 45, 5, 746m, "Deposit" },
                    { 46, 5, 73m, "Deposit" },
                    { 47, 5, 753m, "Deposit" },
                    { 48, 5, 471m, "Deposit" },
                    { 49, 5, 933m, "Deposit" },
                    { 50, 5, 543m, "Withdraw" },
                    { 51, 5, 958m, "Withdraw" },
                    { 52, 5, 106m, "Deposit" },
                    { 53, 6, 752m, "Withdraw" },
                    { 54, 6, 173m, "Deposit" },
                    { 55, 6, 931m, "Deposit" },
                    { 56, 6, 557m, "Withdraw" },
                    { 57, 6, 824m, "Withdraw" },
                    { 58, 6, 67m, "Deposit" },
                    { 59, 6, 595m, "Withdraw" },
                    { 60, 6, 642m, "Withdraw" },
                    { 61, 7, 419m, "Deposit" },
                    { 62, 7, 516m, "Deposit" },
                    { 63, 7, 256m, "Deposit" },
                    { 64, 7, 409m, "Withdraw" },
                    { 65, 7, 912m, "Deposit" },
                    { 66, 7, 531m, "Withdraw" },
                    { 67, 8, 569m, "Deposit" },
                    { 68, 8, 587m, "Deposit" },
                    { 69, 8, 232m, "Deposit" },
                    { 70, 8, 622m, "Deposit" },
                    { 71, 8, 673m, "Deposit" },
                    { 72, 8, 151m, "Deposit" },
                    { 73, 8, 35m, "Deposit" },
                    { 74, 8, 475m, "Deposit" },
                    { 75, 8, 171m, "Deposit" },
                    { 76, 8, 886m, "Withdraw" },
                    { 77, 8, 564m, "Withdraw" },
                    { 78, 9, 86m, "Deposit" },
                    { 79, 9, 855m, "Deposit" },
                    { 80, 9, 573m, "Withdraw" },
                    { 81, 9, 374m, "Deposit" },
                    { 82, 9, 547m, "Withdraw" },
                    { 83, 9, 157m, "Deposit" },
                    { 84, 9, 889m, "Deposit" },
                    { 85, 9, 927m, "Deposit" },
                    { 86, 9, 661m, "Deposit" },
                    { 87, 10, 287m, "Deposit" },
                    { 88, 10, 693m, "Withdraw" },
                    { 89, 10, 584m, "Withdraw" },
                    { 90, 10, 987m, "Withdraw" },
                    { 91, 10, 740m, "Deposit" },
                    { 92, 10, 67m, "Withdraw" },
                    { 93, 10, 392m, "Deposit" },
                    { 94, 10, 889m, "Deposit" },
                    { 95, 11, 978m, "Deposit" },
                    { 96, 11, 396m, "Deposit" },
                    { 97, 11, 527m, "Deposit" },
                    { 98, 11, 862m, "Deposit" },
                    { 99, 11, 390m, "Deposit" },
                    { 100, 11, 360m, "Withdraw" },
                    { 101, 11, 816m, "Deposit" },
                    { 102, 11, 52m, "Withdraw" },
                    { 103, 12, 253m, "Deposit" },
                    { 104, 12, 232m, "Deposit" },
                    { 105, 12, 978m, "Deposit" },
                    { 106, 12, 573m, "Withdraw" },
                    { 107, 12, 384m, "Deposit" },
                    { 108, 12, 287m, "Deposit" },
                    { 109, 12, 968m, "Deposit" },
                    { 110, 12, 468m, "Withdraw" },
                    { 111, 12, 909m, "Deposit" },
                    { 112, 12, 288m, "Deposit" },
                    { 113, 12, 119m, "Withdraw" },
                    { 114, 12, 745m, "Deposit" },
                    { 115, 12, 281m, "Withdraw" },
                    { 116, 13, 222m, "Withdraw" },
                    { 117, 13, 491m, "Deposit" },
                    { 118, 13, 273m, "Withdraw" },
                    { 119, 13, 794m, "Withdraw" },
                    { 120, 13, 870m, "Withdraw" },
                    { 121, 14, 677m, "Deposit" },
                    { 122, 14, 869m, "Deposit" },
                    { 123, 14, 367m, "Withdraw" },
                    { 124, 14, 396m, "Deposit" },
                    { 125, 14, 549m, "Withdraw" },
                    { 126, 14, 768m, "Withdraw" },
                    { 127, 14, 787m, "Deposit" },
                    { 128, 14, 950m, "Deposit" },
                    { 129, 14, 158m, "Deposit" },
                    { 130, 14, 876m, "Deposit" },
                    { 131, 14, 968m, "Withdraw" },
                    { 132, 14, 233m, "Deposit" },
                    { 133, 15, 188m, "Deposit" },
                    { 134, 15, 244m, "Deposit" },
                    { 135, 15, 345m, "Deposit" },
                    { 136, 15, 839m, "Withdraw" },
                    { 137, 15, 689m, "Withdraw" },
                    { 138, 16, 523m, "Withdraw" },
                    { 139, 16, 976m, "Withdraw" },
                    { 140, 16, 127m, "Deposit" },
                    { 141, 16, 462m, "Withdraw" },
                    { 142, 16, 112m, "Withdraw" },
                    { 143, 16, 988m, "Withdraw" },
                    { 144, 16, 317m, "Withdraw" },
                    { 145, 16, 985m, "Withdraw" },
                    { 146, 16, 349m, "Deposit" },
                    { 147, 16, 634m, "Deposit" }
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
