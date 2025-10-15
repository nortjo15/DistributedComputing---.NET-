using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankWebService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User Profiles",
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
                    table.PrimaryKey("PK_User Profiles", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Bank Accounts",
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
                    table.PrimaryKey("PK_Bank Accounts", x => x.AccountNumber);
                    table.ForeignKey(
                        name: "FK_Bank Accounts_User Profiles_Username",
                        column: x => x.Username,
                        principalTable: "User Profiles",
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
                        name: "FK_Transactions_Bank Accounts_AccountNumber",
                        column: x => x.AccountNumber,
                        principalTable: "Bank Accounts",
                        principalColumn: "AccountNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bank Accounts_Username",
                table: "Bank Accounts",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountNumber",
                table: "Transactions",
                column: "AccountNumber");

            migrationBuilder.CreateIndex(
                name: "IX_User Profiles_Email",
                table: "User Profiles",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Bank Accounts");

            migrationBuilder.DropTable(
                name: "User Profiles");
        }
    }
}
