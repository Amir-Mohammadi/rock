using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class mergemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShopStuffPrices_StuffId",
                table: "ShopStuffPrices");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ShopStuffPrices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffPrices_StuffId_ColorId",
                table: "ShopStuffPrices",
                columns: new[] { "StuffId", "ColorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_ProductColors_StuffId_ColorId",
                table: "ShopStuffPrices",
                columns: new[] { "StuffId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_ProductColors_StuffId_ColorId",
                table: "ShopStuffPrices");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffPrices_StuffId_ColorId",
                table: "ShopStuffPrices");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ShopStuffPrices");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffPrices_StuffId",
                table: "ShopStuffPrices",
                column: "StuffId");
        }
    }
}
