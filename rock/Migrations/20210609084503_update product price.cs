using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class updateproductprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "ProductPrices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "ProductPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
