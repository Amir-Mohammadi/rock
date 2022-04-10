using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class fixpayment : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_FinancialAccounts_Profiles_ProfileId1",
          table: "FinancialAccounts");
      migrationBuilder.DropForeignKey(
          name: "FK_OrderPayments_Coupons_CouponId",
          table: "OrderPayments");
      migrationBuilder.DropPrimaryKey(
          name: "PK_OrderPayments",
          table: "OrderPayments");
      migrationBuilder.DropIndex(
          name: "IX_OrderPayments_CouponId",
          table: "OrderPayments");
      migrationBuilder.DropIndex(
          name: "IX_FinancialAccounts_ProfileId1",
          table: "FinancialAccounts");
      migrationBuilder.DropColumn(
          name: "CouponId",
          table: "OrderPayments");
      migrationBuilder.DropColumn(
          name: "Expiration",
          table: "OrderPayments");
      migrationBuilder.DropColumn(
          name: "PayableAmount",
          table: "OrderPayments");
      migrationBuilder.DropColumn(
          name: "ProfileId1",
          table: "FinancialAccounts");
      migrationBuilder.RenameColumn(
          name: "TotalAmount",
          table: "OrderPayments",
          newName: "Amount");
      migrationBuilder.RenameColumn(
          name: "Tax",
          table: "OrderPayments",
          newName: "Id");
      migrationBuilder.RenameColumn(
          name: "Previewed",
          table: "OrderPayments",
          newName: "Visited");
      migrationBuilder.RenameColumn(
          name: "OrderId",
          table: "BankTransactions",
          newName: "OrderPaymentId");
      migrationBuilder.AddColumn<int>(
          name: "CouponId",
          table: "Orders",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<DateTime>(
          name: "Expiration",
          table: "Orders",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
      migrationBuilder.AddColumn<int>(
          name: "LatestOrderStatusId",
          table: "Orders",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<int>(
          name: "Tax",
          table: "Orders",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<int>(
          name: "TotalAmount",
          table: "Orders",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.DropColumn(
      name: "Id",
      table: "OrderPayments");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrderPayments",
                type: "int",
                nullable: false
                )
                .Annotation("SqlServer:Identity", "1, 1");
      migrationBuilder.AddColumn<int>(
          name: "LatestOrderItemStatusId",
          table: "OrderItems",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<DateTime>(
          name: "CreatedAt",
          table: "FinancialTransactions",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
      migrationBuilder.AddColumn<DateTime>(
          name: "DeletedAt",
          table: "FinancialTransactions",
          type: "datetime2",
          nullable: true);
      migrationBuilder.AddColumn<byte[]>(
          name: "RowVersion",
          table: "FinancialTransactions",
          type: "rowversion",
          rowVersion: true,
          nullable: false);
      migrationBuilder.AddPrimaryKey(
          name: "PK_OrderPayments",
          table: "OrderPayments",
          column: "Id");
      migrationBuilder.CreateTable(
          name: "OrderStatuses",
          columns: table => new
          {
            Id = table.Column<int>(type: "int", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            OrderStatusType = table.Column<int>(type: "int", nullable: false),
            OrderId = table.Column<int>(type: "int", nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            UserId = table.Column<int>(type: "int", nullable: false),
            Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OrderStatuses", x => x.Id);
            table.ForeignKey(
                      name: "FK_OrderStatuses_Orders_OrderId",
                      column: x => x.OrderId,
                      principalTable: "Orders",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_OrderStatuses_Users_UserId",
                      column: x => x.UserId,
                      principalTable: "Users",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateTable(
          name: "PaymentGateways",
          columns: table => new
          {
            Gateway = table.Column<string>(type: "nvarchar(450)", nullable: false),
            CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            FinancialAccountId = table.Column<int>(type: "int", nullable: false),
            DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
            ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
            ImageTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
            ImageAlt = table.Column<string>(type: "nvarchar(max)", nullable: false),
            RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_PaymentGateways", x => x.Gateway);
            table.ForeignKey(
                      name: "FK_PaymentGateways_Files_ImageId",
                      column: x => x.ImageId,
                      principalTable: "Files",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
            table.ForeignKey(
                      name: "FK_PaymentGateways_FinancialAccounts_FinancialAccountId",
                      column: x => x.FinancialAccountId,
                      principalTable: "FinancialAccounts",
                      principalColumn: "Id",
                      onDelete: ReferentialAction.Restrict);
          });
      migrationBuilder.CreateIndex(
          name: "IX_Orders_CouponId",
          table: "Orders",
          column: "CouponId");
      migrationBuilder.CreateIndex(
          name: "IX_Orders_LatestOrderStatusId",
          table: "Orders",
          column: "LatestOrderStatusId",
          unique: true,
          filter: "[LatestOrderStatusId] IS NOT NULL");
      migrationBuilder.CreateIndex(
          name: "IX_OrderPayments_OrderId",
          table: "OrderPayments",
          column: "OrderId");
      migrationBuilder.CreateIndex(
          name: "IX_OrderItems_LatestOrderItemStatusId",
          table: "OrderItems",
          column: "LatestOrderItemStatusId",
          unique: true,
          filter: "[LatestOrderItemStatusId] IS NOT NULL");
      migrationBuilder.CreateIndex(
          name: "IX_OrderStatuses_OrderId",
          table: "OrderStatuses",
          column: "OrderId");
      migrationBuilder.CreateIndex(
          name: "IX_OrderStatuses_UserId",
          table: "OrderStatuses",
          column: "UserId");
      migrationBuilder.CreateIndex(
          name: "IX_PaymentGateways_FinancialAccountId",
          table: "PaymentGateways",
          column: "FinancialAccountId");
      migrationBuilder.CreateIndex(
          name: "IX_PaymentGateways_ImageId",
          table: "PaymentGateways",
          column: "ImageId",
          unique: true);
      migrationBuilder.AddForeignKey(
          name: "FK_OrderItems_OrderItemStatuses_LatestOrderItemStatusId",
          table: "OrderItems",
          column: "LatestOrderItemStatusId",
          principalTable: "OrderItemStatuses",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Orders_Coupons_CouponId",
          table: "Orders",
          column: "CouponId",
          principalTable: "Coupons",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Orders_OrderStatuses_LatestOrderStatusId",
          table: "Orders",
          column: "LatestOrderStatusId",
          principalTable: "OrderStatuses",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_OrderItems_OrderItemStatuses_LatestOrderItemStatusId",
          table: "OrderItems");
      migrationBuilder.DropForeignKey(
          name: "FK_Orders_Coupons_CouponId",
          table: "Orders");
      migrationBuilder.DropForeignKey(
          name: "FK_Orders_OrderStatuses_LatestOrderStatusId",
          table: "Orders");
      migrationBuilder.DropTable(
          name: "OrderStatuses");
      migrationBuilder.DropTable(
          name: "PaymentGateways");
      migrationBuilder.DropIndex(
          name: "IX_Orders_CouponId",
          table: "Orders");
      migrationBuilder.DropIndex(
          name: "IX_Orders_LatestOrderStatusId",
          table: "Orders");
      migrationBuilder.DropPrimaryKey(
          name: "PK_OrderPayments",
          table: "OrderPayments");
      migrationBuilder.DropIndex(
          name: "IX_OrderPayments_OrderId",
          table: "OrderPayments");
      migrationBuilder.DropIndex(
          name: "IX_OrderItems_LatestOrderItemStatusId",
          table: "OrderItems");
      migrationBuilder.DropColumn(
          name: "CouponId",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "Expiration",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "LatestOrderStatusId",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "Tax",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "TotalAmount",
          table: "Orders");
      migrationBuilder.DropColumn(
          name: "LatestOrderItemStatusId",
          table: "OrderItems");
      migrationBuilder.DropColumn(
          name: "CreatedAt",
          table: "FinancialTransactions");
      migrationBuilder.DropColumn(
          name: "DeletedAt",
          table: "FinancialTransactions");
      migrationBuilder.DropColumn(
          name: "RowVersion",
          table: "FinancialTransactions");
      migrationBuilder.RenameColumn(
          name: "Visited",
          table: "OrderPayments",
          newName: "Previewed");
      migrationBuilder.RenameColumn(
          name: "Amount",
          table: "OrderPayments",
          newName: "TotalAmount");
      migrationBuilder.RenameColumn(
          name: "Id",
          table: "OrderPayments",
          newName: "Tax");
      migrationBuilder.RenameColumn(
          name: "OrderPaymentId",
          table: "BankTransactions",
          newName: "OrderId");
      migrationBuilder.AlterColumn<int>(
          name: "Tax",
          table: "OrderPayments",
          type: "int",
          nullable: false,
          oldClrType: typeof(int),
          oldType: "int")
          .OldAnnotation("SqlServer:Identity", "1, 1");
      migrationBuilder.AddColumn<int>(
          name: "CouponId",
          table: "OrderPayments",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<DateTime>(
          name: "Expiration",
          table: "OrderPayments",
          type: "datetime2",
          nullable: false,
          defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
      migrationBuilder.AddColumn<int>(
          name: "PayableAmount",
          table: "OrderPayments",
          type: "int",
          nullable: false,
          defaultValue: 0);
      migrationBuilder.AddColumn<int>(
          name: "ProfileId1",
          table: "FinancialAccounts",
          type: "int",
          nullable: true);
      migrationBuilder.AddPrimaryKey(
          name: "PK_OrderPayments",
          table: "OrderPayments",
          column: "OrderId");
      migrationBuilder.CreateIndex(
          name: "IX_OrderPayments_CouponId",
          table: "OrderPayments",
          column: "CouponId");
      migrationBuilder.CreateIndex(
          name: "IX_FinancialAccounts_ProfileId1",
          table: "FinancialAccounts",
          column: "ProfileId1");
      migrationBuilder.AddForeignKey(
          name: "FK_FinancialAccounts_Profiles_ProfileId1",
          table: "FinancialAccounts",
          column: "ProfileId1",
          principalTable: "Profiles",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_OrderPayments_Coupons_CouponId",
          table: "OrderPayments",
          column: "CouponId",
          principalTable: "Coupons",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}