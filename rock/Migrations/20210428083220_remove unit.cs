using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class removeunit : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_CartItems_Units_UnitId",
          table: "CartItems");
      migrationBuilder.DropForeignKey(
          name: "FK_InventoryTransactions_Units_UnitId",
          table: "InventoryTransactions");
      migrationBuilder.DropTable(
          name: "Units");
      migrationBuilder.DropIndex(
          name: "IX_InventoryTransactions_UnitId",
          table: "InventoryTransactions");
      migrationBuilder.DropIndex(
          name: "IX_CartItems_UnitId",
          table: "CartItems");
      migrationBuilder.DropColumn(
          name: "UnitId",
          table: "InventoryTransactions");
      migrationBuilder.DropColumn(
          name: "UnitId",
          table: "CartItems");
      migrationBuilder.RenameColumn(
          name: "PaymenGatewayId",
          table: "Carts",
          newName: "ProfileAddressId");
      migrationBuilder.AddColumn<string>(
          name: "Name",
          table: "Shops",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.AddColumn<bool>(
          name: "IsVerfied",
          table: "Purchases",
          type: "bit",
          nullable: false,
          defaultValue: false);
      migrationBuilder.AddColumn<int>(
          name: "OrderId",
          table: "Purchases",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<string>(
          name: "PaymentPayload",
          table: "Purchases",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "PaymentSapNo",
          table: "Purchases",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "PaymentTransactionNo",
          table: "Purchases",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "RRN",
          table: "Purchases",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "VerifyPayload",
          table: "Purchases",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.AlterColumn<string>(
          name: "Phone",
          table: "Profiles",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");
      migrationBuilder.AlterColumn<string>(
          name: "Email",
          table: "Profiles",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");
      migrationBuilder.AddColumn<bool>(
          name: "IsPublished",
          table: "ProductPrices",
          type: "bit",
          nullable: false,
          defaultValue: false);
      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "PersonProfiles",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");
      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "PersonProfiles",
          type: "nvarchar(max)",
          nullable: true,
          oldClrType: typeof(string),
          oldType: "nvarchar(max)");
      migrationBuilder.AddColumn<int>(
          name: "FinancialDocumentId",
          table: "Orders",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<int>(
          name: "MaxQuantitiesPerUser",
          table: "Coupons",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<int>(
          name: "CartStatus",
          table: "Carts",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.CreateTable(
          name: "BankTransactions",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            OrderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PaymentKind = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PaymentGateway = table.Column<string>(type: "nvarchar(max)", nullable: true),
            TerminalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            CardNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            BankParameter1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            BankParameter2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            BankParameter3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            BankParameter4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Amount = table.Column<int>(type: "int", nullable: false),
            State = table.Column<int>(type: "int", nullable: false),
            UserId = table.Column<int>(type: "int", nullable: false),
            RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_BankTransactions", x => x.Id);
            table.ForeignKey(
                      name: "FK_BankTransactions_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateTable(
          name: "OrderPayments",
          columns: table => new
          {
            OrderId = table.Column<int>(type: "int", nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            Amount = table.Column<int>(type: "int", nullable: false),
            Tax = table.Column<int>(type: "int", nullable: false),
            PaymentTransactionNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PaymentSapNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PaymentPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
            IsVerified = table.Column<bool>(type: "bit", nullable: false),
            VerifyPayload = table.Column<string>(type: "nvarchar(max)", nullable: true),
            RRN = table.Column<string>(type: "nvarchar(max)", nullable: true),
            PaymentStatus = table.Column<int>(type: "int", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OrderPayments", x => x.OrderId);
            table.ForeignKey(
                      name: "FK_OrderPayments_Orders_OrderId",
                      column: x => x.OrderId,
                      principalTable: "Orders",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateIndex(
          name: "IX_Purchases_OrderId",
          table: "Purchases",
          column: "OrderId");
      migrationBuilder.CreateIndex(
          name: "IX_Orders_FinancialDocumentId",
          table: "Orders",
          column: "FinancialDocumentId");
      migrationBuilder.CreateIndex(
          name: "IX_Carts_ProfileAddressId",
          table: "Carts",
          column: "ProfileAddressId");
      migrationBuilder.CreateIndex(
          name: "IX_BankTransactions_UserId",
          table: "BankTransactions",
          column: "UserId");
      migrationBuilder.AddForeignKey(
          name: "FK_Carts_ProfileAddresses_ProfileAddressId",
          table: "Carts",
          column: "ProfileAddressId",
          principalTable: "ProfileAddresses",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Orders_FinancialDocuments_FinancialDocumentId",
          table: "Orders",
          column: "FinancialDocumentId",
          principalTable: "FinancialDocuments",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Purchases_Orders_OrderId",
          table: "Purchases",
          column: "OrderId",
          principalTable: "Orders",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Carts_ProfileAddresses_ProfileAddressId",
          table: "Carts");
      migrationBuilder.DropForeignKey(
          name: "FK_Orders_FinancialDocuments_FinancialDocumentId",
          table: "Orders");
      migrationBuilder.DropForeignKey(
          name: "FK_Purchases_Orders_OrderId",
          table: "Purchases");
      migrationBuilder.DropTable(
          name: "BankTransactions");
      migrationBuilder.DropTable(
          name: "OrderPayments");
      migrationBuilder.DropIndex(
          name: "IX_Purchases_OrderId",
          table: "Purchases");
      migrationBuilder.DropIndex(
          name: "IX_Orders_FinancialDocumentId",
          table: "Orders");
      migrationBuilder.DropIndex(
          name: "IX_Carts_ProfileAddressId",
          table: "Carts");
      migrationBuilder.DropColumn(
          name: "Name",
          table: "Shops");
      migrationBuilder.DropColumn(
          name: "IsVerfied",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "OrderId",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "PaymentPayload",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "PaymentSapNo",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "PaymentTransactionNo",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "RRN",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "VerifyPayload",
          table: "Purchases");
      migrationBuilder.DropColumn(
          name: "IsPublished",
          table: "ProductPrices");
      migrationBuilder.DropColumn(
          name: "FinancialDocumentId",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "MaxQuantitiesPerUser",
          table: "Coupons");
      migrationBuilder.DropColumn(
          name: "CartStatus",
          table: "Carts");
      // migrationBuilder.RenameColumn(
      //     name: "ProfileAddressId",
      //     table: "Carts",
      //     newName: "PaymenGatewayId");
      migrationBuilder.AlterColumn<string>(
          name: "Phone",
          table: "Profiles",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
      migrationBuilder.AlterColumn<string>(
          name: "Email",
          table: "Profiles",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
      migrationBuilder.AlterColumn<string>(
          name: "LastName",
          table: "PersonProfiles",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
      migrationBuilder.AlterColumn<string>(
          name: "FirstName",
          table: "PersonProfiles",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "",
          oldClrType: typeof(string),
          oldType: "nvarchar(max)",
          oldNullable: true);
      migrationBuilder.AddColumn<int>(
          name: "UnitId",
          table: "InventoryTransactions",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<int>(
          name: "UnitId",
          table: "CartItems",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.CreateTable(
          name: "Units",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
            Ratio = table.Column<double>(type: "float", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
            Type = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Units", x => x.Id);
          });
      migrationBuilder.CreateIndex(
          name: "IX_InventoryTransactions_UnitId",
          table: "InventoryTransactions",
          column: "UnitId");
      migrationBuilder.CreateIndex(
          name: "IX_CartItems_UnitId",
          table: "CartItems",
          column: "UnitId");
      migrationBuilder.AddForeignKey(
          name: "FK_CartItems_Units_UnitId",
          table: "CartItems",
          column: "UnitId",
          principalTable: "Units",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_InventoryTransactions_Units_UnitId",
          table: "InventoryTransactions",
          column: "UnitId",
          principalTable: "Units",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}