using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class addhasmultiplevaluetocatalogitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasMultiple",
                table: "CatalogItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasMultiple",
                table: "CatalogItems");
        }
    }
}
