using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class fixprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "Profiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Profiles",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "PersonProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "PersonProfiles",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "NationalCode",
                table: "PersonProfiles");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "PersonProfiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CompanyProfiles");

            migrationBuilder.AddColumn<string>(
                name: "NationalCode",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
