using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class addshowinfiltertocatalogitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowInFilter",
                table: "CatalogItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowInFilter",
                table: "CatalogItems");
        }
    }
}
