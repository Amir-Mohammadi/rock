using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
  public partial class filetag : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.RenameColumn(
          name: "UpdateAt",
          table: "BankTransactions",
          newName: "UpdatedAt");

      migrationBuilder.AddColumn<int>(
          name: "ThreadId",
          table: "Files",
          type: "int",
          nullable: true);

      migrationBuilder.DropColumn(
         name: "RowVersion",
         table: "BankTransactions");


      migrationBuilder.AddColumn<byte[]>(
          name: "RowVersion",
          table: "BankTransactions",
          type: "rowversion",
          rowVersion: true,
          nullable: false);

      migrationBuilder.CreateIndex(
          name: "IX_Files_ThreadId",
          table: "Files",
          column: "ThreadId",
          unique: true,
          filter: "[ThreadId] IS NOT NULL");

      migrationBuilder.AddForeignKey(
          name: "FK_Files_Threads_ThreadId",
          table: "Files",
          column: "ThreadId",
          principalTable: "Threads",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Files_Threads_ThreadId",
          table: "Files");

      migrationBuilder.DropIndex(
          name: "IX_Files_ThreadId",
          table: "Files");

      migrationBuilder.DropColumn(
          name: "ThreadId",
          table: "Files");

      migrationBuilder.RenameColumn(
          name: "UpdatedAt",
          table: "BankTransactions",
          newName: "UpdateAt");

      migrationBuilder.AlterColumn<byte[]>(
          name: "RowVersion",
          table: "BankTransactions",
          type: "varbinary(max)",
          nullable: true,
          oldClrType: typeof(byte[]),
          oldType: "rowversion",
          oldRowVersion: true);
    }
  }
}
