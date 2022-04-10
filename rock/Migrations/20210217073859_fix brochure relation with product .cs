using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class fixbrochurerelationwithproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrochureAttachments_ProductBrochures_ProductBrochurId",
                table: "ProductBrochureAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductBrochures_BrochureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrochureId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrochures_ProductId",
                table: "ProductBrochures");

            migrationBuilder.DropColumn(
                name: "BrochureId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductBrochurId",
                table: "ProductBrochureAttachments",
                newName: "BrochurId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBrochureAttachments_ProductBrochurId",
                table: "ProductBrochureAttachments",
                newName: "IX_ProductBrochureAttachments_BrochurId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrochures_ProductId",
                table: "ProductBrochures",
                column: "ProductId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrochureAttachments_ProductBrochures_BrochurId",
                table: "ProductBrochureAttachments",
                column: "BrochurId",
                principalTable: "ProductBrochures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductBrochureAttachments_ProductBrochures_BrochurId",
                table: "ProductBrochureAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ProductBrochures_ProductId",
                table: "ProductBrochures");

            migrationBuilder.RenameColumn(
                name: "BrochurId",
                table: "ProductBrochureAttachments",
                newName: "ProductBrochurId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductBrochureAttachments_BrochurId",
                table: "ProductBrochureAttachments",
                newName: "IX_ProductBrochureAttachments_ProductBrochurId");

            migrationBuilder.AddColumn<int>(
                name: "BrochureId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrochureId",
                table: "Products",
                column: "BrochureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBrochures_ProductId",
                table: "ProductBrochures",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBrochureAttachments_ProductBrochures_ProductBrochurId",
                table: "ProductBrochureAttachments",
                column: "ProductBrochurId",
                principalTable: "ProductBrochures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductBrochures_BrochureId",
                table: "Products",
                column: "BrochureId",
                principalTable: "ProductBrochures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
