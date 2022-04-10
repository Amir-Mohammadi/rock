using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class updateshop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopProfiles_Cities_CityId",
                table: "ShopProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ShopProfiles_CityId",
                table: "ShopProfiles");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "ShopProfiles");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Shops",
                newName: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Shops_CityId",
                table: "Shops",
                column: "CityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shops_Cities_CityId",
                table: "Shops",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shops_Cities_CityId",
                table: "Shops");

            migrationBuilder.DropIndex(
                name: "IX_Shops_CityId",
                table: "Shops");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Shops",
                newName: "Code");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "ShopProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShopProfiles_CityId",
                table: "ShopProfiles",
                column: "CityId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopProfiles_Cities_CityId",
                table: "ShopProfiles",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
