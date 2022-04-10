using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class removeNumericalValuefromCatalogMemoryItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumericalValue",
                table: "CatalogMemoryItems");

            migrationBuilder.RenameColumn(
                name: "TextValue",
                table: "CatalogMemoryItems",
                newName: "Value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "CatalogMemoryItems",
                newName: "TextValue");

            migrationBuilder.AddColumn<decimal>(
                name: "NumericalValue",
                table: "CatalogMemoryItems",
                type: "decimal(25,9)",
                precision: 25,
                scale: 9,
                nullable: false,
                defaultValue: 0m);
        }
    }
}
