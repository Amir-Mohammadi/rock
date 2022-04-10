using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class fixmappings : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: "RowVersion",
        table: "Profiles");
      migrationBuilder.DropColumn(
        name: "RowVersion",
        table: "PersonProfiles"
      );
      migrationBuilder.AddColumn<DateTime>(
          name: "DeletedAt",
          table: "Products",
          type: "datetime2",
          nullable: true);

      migrationBuilder.AddColumn<byte[]>(
          name: "RowVersion",
          table: "CompanyProfiles",
          type: "rowversion",
          rowVersion: true,
          nullable: false);

      migrationBuilder.AddColumn<byte[]>(
        name: "RowVersion",
        table: "Profiles",
        type: "rowversion",
        rowVersion: true,
        nullable: false);

      migrationBuilder.AddColumn<byte[]>(
        name: "RowVersion",
        table: "PersonProfiles",
        type: "rowversion",
        rowVersion: true,
        nullable: false);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
          name: "DeletedAt",
          table: "Products");
      migrationBuilder.DropColumn(
          name: "RowVersion",
          table: "CompanyProfiles");
      migrationBuilder.DropColumn(
        name: "RowVersion",
        table: "Profiles");
      migrationBuilder.DropColumn(
        name: "RowVersion",
        table: "PersonProfiles");
    }
  }
}