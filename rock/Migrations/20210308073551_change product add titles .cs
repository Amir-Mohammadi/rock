using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class changeproductaddtitles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AltTitle",
                table: "Products",
                newName: "UrlTitle");

            migrationBuilder.AddColumn<string>(
                name: "BrowserTitle",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserTitle",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "UrlTitle",
                table: "Products",
                newName: "AltTitle");
        }
    }
}
