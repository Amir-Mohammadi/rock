using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class fixproductcolorrelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductColors_ProductColorId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductColors_ProductColorId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductColors_DefaultProductColorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductColorId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_ProductColorId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_Products_DefaultProductColorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductColorId",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductColorId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ProductColorId",
                table: "ShopStuffs",
                newName: "ColorId");

            migrationBuilder.RenameColumn(
                name: "DefaultProductColorId",
                table: "Products",
                newName: "DefaultColorId");

            migrationBuilder.RenameColumn(
                name: "ProductColorId",
                table: "ProductPrices",
                newName: "ColorId");

            migrationBuilder.RenameColumn(
                name: "ProductColorId",
                table: "CartItems",
                newName: "ColorId");

            // migrationBuilder.AlterColumn<int>(
            //     name: "Id",
            //     table: "Products",
            //     type: "int",
            //     nullable: false,
            //     oldClrType: typeof(int),
            //     oldType: "int");
                //.OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors",
                columns: new[] { "ProductId", "ColorId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_ProductId_ColorId",
                table: "ShopStuffs",
                columns: new[] { "ProductId", "ColorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id_DefaultColorId",
                table: "Products",
                columns: new[] { "Id", "DefaultColorId" },
                unique: true,
                filter: "[DefaultColorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId_ColorId",
                table: "ProductPrices",
                columns: new[] { "ProductId", "ColorId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId_ColorId",
                table: "CartItems",
                columns: new[] { "ProductId", "ColorId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductColors_ProductId_ColorId",
                table: "CartItems",
                columns: new[] { "ProductId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductColors_ProductId_ColorId",
                table: "ProductPrices",
                columns: new[] { "ProductId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductColors_Id_DefaultColorId",
                table: "Products",
                columns: new[] { "Id", "DefaultColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductId_ColorId",
                table: "ShopStuffs",
                columns: new[] { "ProductId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ProductColors_ProductId_ColorId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_ProductColors_ProductId_ColorId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductColors_Id_DefaultColorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductId_ColorId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_ProductId_ColorId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_Products_Id_DefaultColorId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId_ColorId",
                table: "ProductPrices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_ProductColors_ColorId",
                table: "ProductColors");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ProductId_ColorId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "ShopStuffs",
                newName: "ProductColorId");

            migrationBuilder.RenameColumn(
                name: "DefaultColorId",
                table: "Products",
                newName: "DefaultProductColorId");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "ProductPrices",
                newName: "ProductColorId");

            migrationBuilder.RenameColumn(
                name: "ColorId",
                table: "CartItems",
                newName: "ProductColorId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductColors",
                table: "ProductColors",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_ProductColorId",
                table: "ShopStuffs",
                column: "ProductColorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_DefaultProductColorId",
                table: "Products",
                column: "DefaultProductColorId",
                unique: true,
                filter: "[DefaultProductColorId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductColorId",
                table: "ProductPrices",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColors_ProductId",
                table: "ProductColors",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductColorId",
                table: "CartItems",
                column: "ProductColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ProductColors_ProductColorId",
                table: "CartItems",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_ProductColors_ProductColorId",
                table: "ProductPrices",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductColors_DefaultProductColorId",
                table: "Products",
                column: "DefaultProductColorId",
                principalTable: "ProductColors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductColorId",
                table: "ShopStuffs",
                column: "ProductColorId",
                principalTable: "ProductColors",
                principalColumn: "ColorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
