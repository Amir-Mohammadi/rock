using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class Changeinventorytowarehouse : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Shops_Inventories_InventoryId",
          table: "Shops");
      migrationBuilder.DropTable(
          name: "InventoryTransactions");
      migrationBuilder.DropTable(
          name: "Inventories");
      migrationBuilder.DropColumn(
          name: "Usage",
          table: "Coupons");
      migrationBuilder.RenameColumn(
          name: "InventoryId",
          table: "Shops",
          newName: "WarehouseId");
      migrationBuilder.RenameIndex(
          name: "IX_Shops_InventoryId",
          table: "Shops",
          newName: "IX_Shops_WarehouseId");
      migrationBuilder.DropColumn(
                     name: "RowVersion",
                     table: "ShopInventoryModifications");
      migrationBuilder.AddColumn<byte[]>(
name: "RowVersion",
table: "ShopInventoryModifications",
type: "rowversion",
rowVersion: true,
nullable: false);
      // migrationBuilder.AlterColumn<byte[]>(
      //     name: "RowVersion",
      //     table: "ShopInventoryModifications",
      //     type: "rowversion",
      //     rowVersion: true,
      //     nullable: false,
      //     defaultValue: new byte[0],
      //     oldClrType: typeof(byte[]),
      //     oldType: "varbinary(max)",
      //     oldNullable: true);
      migrationBuilder.AddColumn<DateTime>(
          name: "Expiration",
          table: "OrderPayments",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
      migrationBuilder.AlterColumn<int>(
          name: "Amount",
          table: "FinancialTransactions",
          type: "int",
          precision: 25,
          scale: 9,
          nullable: false,
          oldClrType: typeof(decimal),
          oldType: "decimal(25,9)",
          oldPrecision: 25,
          oldScale: 9);
      migrationBuilder.CreateTable(
          name: "Warehouses",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Warehouses", x => x.Id);
          });
      migrationBuilder.InsertData("Warehouses", "Name", "test");
      migrationBuilder.CreateTable(
          name: "WarehouseTransactions",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            ProductId = table.Column<int>(type: "int", nullable: false),
            ColorId = table.Column<int>(type: "int", nullable: false),
            Factor = table.Column<int>(type: "int", nullable: false),
            Amount = table.Column<int>(type: "int", precision: 25, scale: 9, nullable: false),
            DocumentId = table.Column<int>(type: "int", nullable: false),
            WarehouseId = table.Column<int>(type: "int", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_WarehouseTransactions", x => x.Id);
            table.ForeignKey(
                      name: "FK_WarehouseTransactions_Colors_ColorId",
                      column: x => x.ColorId,
                      principalTable: "Colors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_WarehouseTransactions_Documents_DocumentId",
                      column: x => x.DocumentId,
                      principalTable: "Documents",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_WarehouseTransactions_ProductColors_ProductId_ColorId",
                      columns: x => new { x.ProductId, x.ColorId },
                      principalTable: "ProductColors",
                      principalColumns: new[] { "ProductId", "ColorId" },
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_WarehouseTransactions_Products_ProductId",
                      column: x => x.ProductId,
                      principalTable: "Products",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_WarehouseTransactions_Warehouses_WarehouseId",
                      column: x => x.WarehouseId,
                      principalTable: "Warehouses",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateIndex(
          name: "IX_WarehouseTransactions_ColorId",
          table: "WarehouseTransactions",
          column: "ColorId");
      migrationBuilder.CreateIndex(
          name: "IX_WarehouseTransactions_DocumentId",
          table: "WarehouseTransactions",
          column: "DocumentId");
      migrationBuilder.CreateIndex(
          name: "IX_WarehouseTransactions_ProductId_ColorId",
          table: "WarehouseTransactions",
          columns: new[] { "ProductId", "ColorId" });
      migrationBuilder.CreateIndex(
          name: "IX_WarehouseTransactions_WarehouseId",
          table: "WarehouseTransactions",
          column: "WarehouseId");
      migrationBuilder.AddForeignKey(
          name: "FK_Shops_Warehouses_WarehouseId",
          table: "Shops",
          column: "WarehouseId",
          principalTable: "Warehouses",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Shops_Warehouses_WarehouseId",
          table: "Shops");
      migrationBuilder.DropTable(
          name: "WarehouseTransactions");
      migrationBuilder.DropTable(
          name: "Warehouses");
      migrationBuilder.DropColumn(
          name: "Expiration",
          table: "OrderPayments");
      migrationBuilder.RenameColumn(
          name: "WarehouseId",
          table: "Shops",
          newName: "InventoryId");
      migrationBuilder.RenameIndex(
          name: "IX_Shops_WarehouseId",
          table: "Shops",
          newName: "IX_Shops_InventoryId");
      migrationBuilder.AlterColumn<byte[]>(
          name: "RowVersion",
          table: "ShopInventoryModifications",
          type: "varbinary(max)",
          nullable: true,
          oldClrType: typeof(byte[]),
          oldType: "rowversion",
          oldRowVersion: true);
      migrationBuilder.AlterColumn<decimal>(
          name: "Amount",
          table: "FinancialTransactions",
          type: "decimal(25,9)",
          precision: 25,
          scale: 9,
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int",
          oldPrecision: 25,
          oldScale: 9);
      migrationBuilder.AddColumn<int>(
          name: "Usage",
          table: "Coupons",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.CreateTable(
          name: "Inventories",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Inventories", x => x.Id);
          });
      migrationBuilder.CreateTable(
          name: "InventoryTransactions",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Amount = table.Column<decimal>(type: "decimal(25,9)", precision: 25, scale: 9, nullable: false),
            ColorId = table.Column<int>(type: "int", nullable: false),
            DocumentId = table.Column<int>(type: "int", nullable: false),
            Factor = table.Column<int>(type: "int", nullable: false),
            InventoryId = table.Column<int>(type: "int", nullable: false),
            ProductId = table.Column<int>(type: "int", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
            table.ForeignKey(
                      name: "FK_InventoryTransactions_Colors_ColorId",
                      column: x => x.ColorId,
                      principalTable: "Colors",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_InventoryTransactions_Documents_DocumentId",
                      column: x => x.DocumentId,
                      principalTable: "Documents",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_InventoryTransactions_Inventories_InventoryId",
                      column: x => x.InventoryId,
                      principalTable: "Inventories",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_InventoryTransactions_ProductColors_ProductId_ColorId",
                      columns: x => new { x.ProductId, x.ColorId },
                      principalTable: "ProductColors",
                      principalColumns: new[] { "ProductId", "ColorId" },
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_InventoryTransactions_Products_ProductId",
                      column: x => x.ProductId,
                      principalTable: "Products",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateIndex(
          name: "IX_InventoryTransactions_ColorId",
          table: "InventoryTransactions",
          column: "ColorId");
      migrationBuilder.CreateIndex(
          name: "IX_InventoryTransactions_DocumentId",
          table: "InventoryTransactions",
          column: "DocumentId");
      migrationBuilder.CreateIndex(
          name: "IX_InventoryTransactions_InventoryId",
          table: "InventoryTransactions",
          column: "InventoryId");
      migrationBuilder.CreateIndex(
          name: "IX_InventoryTransactions_ProductId_ColorId",
          table: "InventoryTransactions",
          columns: new[] { "ProductId", "ColorId" });
      migrationBuilder.AddForeignKey(
          name: "FK_Shops_Inventories_InventoryId",
          table: "Shops",
          column: "InventoryId",
          principalTable: "Inventories",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}