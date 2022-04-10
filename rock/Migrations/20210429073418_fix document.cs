using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class fixdocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_FinancialDocuments_FinancialDocumentId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_FinancialDocuments_DocumentId",
                table: "FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_InventoryDocuments_DocumentId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_InventoryDocuments_InventoryDocumentId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_FinancialDocuments_FinancialDocumentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_FinancialDocuments_FinancialDocumentId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_InventoryDocuments_InventoryDocumentId",
                table: "Shippings");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModifications_InventoryDocuments_InventoryDocumentId",
                table: "ShopInventoryModifications");

            migrationBuilder.DropTable(
                name: "FinancialDocuments");

            migrationBuilder.DropTable(
                name: "InventoryDocuments");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_InventoryDocumentId",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "InventoryDocumentId",
                table: "InventoryTransactions");

            migrationBuilder.RenameColumn(
                name: "InventoryDocumentId",
                table: "ShopInventoryModifications",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopInventoryModifications_InventoryDocumentId",
                table: "ShopInventoryModifications",
                newName: "IX_ShopInventoryModifications_DocumentId");

            migrationBuilder.RenameColumn(
                name: "InventoryDocumentId",
                table: "Shippings",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shippings_InventoryDocumentId",
                table: "Shippings",
                newName: "IX_Shippings_DocumentId");

            migrationBuilder.RenameColumn(
                name: "FinancialDocumentId",
                table: "Purchases",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_FinancialDocumentId",
                table: "Purchases",
                newName: "IX_Purchases_DocumentId");

            migrationBuilder.RenameColumn(
                name: "FinancialDocumentId",
                table: "Orders",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_FinancialDocumentId",
                table: "Orders",
                newName: "IX_Orders_DocumentId");

            migrationBuilder.RenameColumn(
                name: "FinancialDocumentId",
                table: "Bills",
                newName: "DocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_FinancialDocumentId",
                table: "Bills",
                newName: "IX_Bills_DocumentId");

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_FormId",
                table: "Documents",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Documents_DocumentId",
                table: "Bills",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_Documents_DocumentId",
                table: "FinancialTransactions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_Documents_DocumentId",
                table: "InventoryTransactions",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Documents_DocumentId",
                table: "Orders",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_Documents_DocumentId",
                table: "Purchases",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_Documents_DocumentId",
                table: "Shippings",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModifications_Documents_DocumentId",
                table: "ShopInventoryModifications",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Documents_DocumentId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_Documents_DocumentId",
                table: "FinancialTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_Documents_DocumentId",
                table: "InventoryTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Documents_DocumentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_Documents_DocumentId",
                table: "Purchases");

            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_Documents_DocumentId",
                table: "Shippings");

            migrationBuilder.DropForeignKey(
                name: "FK_ShopInventoryModifications_Documents_DocumentId",
                table: "ShopInventoryModifications");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "ShopInventoryModifications",
                newName: "InventoryDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopInventoryModifications_DocumentId",
                table: "ShopInventoryModifications",
                newName: "IX_ShopInventoryModifications_InventoryDocumentId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Shippings",
                newName: "InventoryDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shippings_DocumentId",
                table: "Shippings",
                newName: "IX_Shippings_InventoryDocumentId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Purchases",
                newName: "FinancialDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Purchases_DocumentId",
                table: "Purchases",
                newName: "IX_Purchases_FinancialDocumentId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Orders",
                newName: "FinancialDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DocumentId",
                table: "Orders",
                newName: "IX_Orders_FinancialDocumentId");

            migrationBuilder.RenameColumn(
                name: "DocumentId",
                table: "Bills",
                newName: "FinancialDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_DocumentId",
                table: "Bills",
                newName: "IX_Bills_FinancialDocumentId");

            migrationBuilder.AddColumn<int>(
                name: "InventoryDocumentId",
                table: "InventoryTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialDocuments_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinancialDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryDocuments_Forms_FormId",
                        column: x => x.FormId,
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDocuments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_InventoryDocumentId",
                table: "InventoryTransactions",
                column: "InventoryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocuments_FormId",
                table: "FinancialDocuments",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialDocuments_UserId",
                table: "FinancialDocuments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDocuments_FormId",
                table: "InventoryDocuments",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDocuments_UserId",
                table: "InventoryDocuments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_FinancialDocuments_FinancialDocumentId",
                table: "Bills",
                column: "FinancialDocumentId",
                principalTable: "FinancialDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_FinancialDocuments_DocumentId",
                table: "FinancialTransactions",
                column: "DocumentId",
                principalTable: "FinancialDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_InventoryDocuments_DocumentId",
                table: "InventoryTransactions",
                column: "DocumentId",
                principalTable: "InventoryDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_InventoryDocuments_InventoryDocumentId",
                table: "InventoryTransactions",
                column: "InventoryDocumentId",
                principalTable: "InventoryDocuments",
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
                name: "FK_Purchases_FinancialDocuments_FinancialDocumentId",
                table: "Purchases",
                column: "FinancialDocumentId",
                principalTable: "FinancialDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_InventoryDocuments_InventoryDocumentId",
                table: "Shippings",
                column: "InventoryDocumentId",
                principalTable: "InventoryDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopInventoryModifications_InventoryDocuments_InventoryDocumentId",
                table: "ShopInventoryModifications",
                column: "InventoryDocumentId",
                principalTable: "InventoryDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
