using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class changefinancialaccountmap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialAccounts_ProfileId",
                table: "FinancialAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccounts_ProfileId",
                table: "FinancialAccounts",
                column: "ProfileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialAccounts_ProfileId",
                table: "FinancialAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccounts_ProfileId",
                table: "FinancialAccounts",
                column: "ProfileId");
        }
    }
}
