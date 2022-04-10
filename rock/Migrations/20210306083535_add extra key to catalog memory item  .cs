using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class addextrakeytocatalogmemoryitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExtraKey",
                table: "CatalogMemoryItems",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtraKey",
                table: "CatalogMemoryItems");
        }
    }
}
