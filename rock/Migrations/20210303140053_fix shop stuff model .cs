using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class fixshopstuffmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_InventoryTransactions_Products_ProductId",
            //     table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_ShopStuffs_ShopStuffId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_ShopStuffs_ShopStuffId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_ShopStuffs_ShopStuffId",
                table: "ShopStuffPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductId_ColorId",
                table: "ShopStuffs");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_ShopStuffs_Products_ProductId",
            //     table: "ShopStuffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopStuffs",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_ProductId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_ProductId_ColorId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_ShopId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopInventoryModificationItems_ShopStuffId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_ProductId",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShopStuffs");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopStuffs");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ShopStuffs",
                newName: "StuffId");

            migrationBuilder.RenameColumn(
                name: "ShopStuffId",
                table: "ShopStuffPrices",
                newName: "StuffId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopStuffPrices_ShopStuffId",
                table: "ShopStuffPrices",
                newName: "IX_ShopStuffPrices_StuffId");

            migrationBuilder.RenameColumn(
                name: "ShopStuffId",
                table: "ShopInventoryModificationItems",
                newName: "StuffId");

            migrationBuilder.RenameColumn(
                name: "ShopStuffId",
                table: "InventoryTransactions",
                newName: "StuffId");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InventoryTransactions",
                newName: "ShopId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransactions_ShopStuffId",
                table: "InventoryTransactions",
                newName: "IX_InventoryTransactions_StuffId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ShopStuffs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopStuffPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "ShopStuffPrices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopInventoryModificationItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShopId",
                table: "ShopInventoryModificationItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopStuffs",
                table: "ShopStuffs",
                columns: new[] { "ShopId", "StuffId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_StuffId",
                table: "ShopStuffs",
                column: "StuffId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffPrices_ColorId",
                table: "ShopStuffPrices",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffPrices_ShopId_StuffId",
                table: "ShopStuffPrices",
                columns: new[] { "ShopId", "StuffId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopInventoryModificationItems_ColorId",
                table: "ShopInventoryModificationItems",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopInventoryModificationItems_ShopId_StuffId",
                table: "ShopInventoryModificationItems",
                columns: new[] { "ShopId", "StuffId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShopInventoryModificationItems_StuffId_ColorId",
                table: "ShopInventoryModificationItems",
                columns: new[] { "StuffId", "ColorId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ColorId",
                table: "ProductPrices",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ShopId_StuffId",
                table: "InventoryTransactions",
                columns: new[] { "ShopId", "StuffId" });

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Products_StuffId",
                table: "InventoryTransactions",
                column: "StuffId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Shops_ShopId",
                table: "InventoryTransactions",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_ShopStuffs_ShopId_StuffId",
                table: "InventoryTransactions",
                columns: new[] { "ShopId", "StuffId" },
                principalTable: "ShopStuffs",
                principalColumns: new[] { "ShopId", "StuffId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductPrices_Colors_ColorId",
                table: "ProductPrices",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_Colors_ColorId",
                table: "ShopInventoryModificationItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_ProductColors_StuffId_ColorId",
                table: "ShopInventoryModificationItems",
                columns: new[] { "StuffId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_Products_StuffId",
                table: "ShopInventoryModificationItems",
                column: "StuffId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_Shops_ShopId",
                table: "ShopInventoryModificationItems",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_ShopStuffs_ShopId_StuffId",
                table: "ShopInventoryModificationItems",
                columns: new[] { "ShopId", "StuffId" },
                principalTable: "ShopStuffs",
                principalColumns: new[] { "ShopId", "StuffId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_Colors_ColorId",
                table: "ShopStuffPrices",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_Products_StuffId",
                table: "ShopStuffPrices",
                column: "StuffId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_Shops_ShopId",
                table: "ShopStuffPrices",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_ShopStuffs_ShopId_StuffId",
                table: "ShopStuffPrices",
                columns: new[] { "ShopId", "StuffId" },
                principalTable: "ShopStuffs",
                principalColumns: new[] { "ShopId", "StuffId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffs_Products_StuffId",
                table: "ShopStuffs",
                column: "StuffId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Products_StuffId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Shops_ShopId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_ShopStuffs_ShopId_StuffId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductPrices_Colors_ColorId",
                table: "ProductPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_Colors_ColorId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_ProductColors_StuffId_ColorId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_Products_StuffId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_Shops_ShopId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModificationItems_ShopStuffs_ShopId_StuffId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_Colors_ColorId",
                table: "ShopStuffPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_Products_StuffId",
                table: "ShopStuffPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_Shops_ShopId",
                table: "ShopStuffPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffPrices_ShopStuffs_ShopId_StuffId",
                table: "ShopStuffPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopStuffs_Products_StuffId",
                table: "ShopStuffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShopStuffs",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffs_StuffId",
                table: "ShopStuffs");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffPrices_ColorId",
                table: "ShopStuffPrices");

            migrationBuilder.DropIndex(
                name: "IX_ShopStuffPrices_ShopId_StuffId",
                table: "ShopStuffPrices");

            migrationBuilder.DropIndex(
                name: "IX_ShopInventoryModificationItems_ColorId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropIndex(
                name: "IX_ShopInventoryModificationItems_ShopId_StuffId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropIndex(
                name: "IX_ShopInventoryModificationItems_StuffId_ColorId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ColorId",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_ShopId_StuffId",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ShopStuffs");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopStuffPrices");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "ShopStuffPrices");

            migrationBuilder.DropColumn(
                name: "ColorId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.DropColumn(
                name: "ShopId",
                table: "ShopInventoryModificationItems");

            migrationBuilder.RenameColumn(
                name: "StuffId",
                table: "ShopStuffs",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "StuffId",
                table: "ShopStuffPrices",
                newName: "ShopStuffId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopStuffPrices_StuffId",
                table: "ShopStuffPrices",
                newName: "IX_ShopStuffPrices_ShopStuffId");

            migrationBuilder.RenameColumn(
                name: "StuffId",
                table: "ShopInventoryModificationItems",
                newName: "ShopStuffId");

            migrationBuilder.RenameColumn(
                name: "StuffId",
                table: "InventoryTransactions",
                newName: "ShopStuffId");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "InventoryTransactions",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransactions_StuffId",
                table: "InventoryTransactions",
                newName: "IX_InventoryTransactions_ShopStuffId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShopStuffs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ColorId",
                table: "ShopStuffs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShopStuffs",
                table: "ShopStuffs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_ProductId",
                table: "ShopStuffs",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_ProductId_ColorId",
                table: "ShopStuffs",
                columns: new[] { "ProductId", "ColorId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopStuffs_ShopId",
                table: "ShopStuffs",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopInventoryModificationItems_ShopStuffId",
                table: "ShopInventoryModificationItems",
                column: "ShopStuffId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_ProductId",
                table: "InventoryTransactions",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Products_ProductId",
                table: "InventoryTransactions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_ShopStuffs_ShopStuffId",
                table: "InventoryTransactions",
                column: "ShopStuffId",
                principalTable: "ShopStuffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModificationItems_ShopStuffs_ShopStuffId",
                table: "ShopInventoryModificationItems",
                column: "ShopStuffId",
                principalTable: "ShopStuffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffPrices_ShopStuffs_ShopStuffId",
                table: "ShopStuffPrices",
                column: "ShopStuffId",
                principalTable: "ShopStuffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffs_ProductColors_ProductId_ColorId",
                table: "ShopStuffs",
                columns: new[] { "ProductId", "ColorId" },
                principalTable: "ProductColors",
                principalColumns: new[] { "ProductId", "ColorId" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopStuffs_Products_ProductId",
                table: "ShopStuffs",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
