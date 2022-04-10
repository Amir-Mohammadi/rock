using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class changebrand : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_CompanyProfiles_CompanyProfileId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_PersonProfiles_PersonProfileId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_CompanyProfileId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_PersonProfileId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "CompanyProfileId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "PersonProfileId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CompanyProfiles");

            migrationBuilder.RenameColumn(
                name: "Stream",
                table: "Files",
                newName: "FileStream");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "FileType");

            migrationBuilder.RenameColumn(
                name: "Ext",
                table: "Files",
                newName: "FileName");

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "PersonProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Seo",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileId",
                table: "CompanyProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Brands",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PersonProfiles_ProfileId",
                table: "PersonProfiles",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfiles_ProfileId",
                table: "CompanyProfiles",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_ImageId",
                table: "Brands",
                column: "ImageId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Files_ImageId",
                table: "Brands",
                column: "ImageId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfiles_Profiles_ProfileId",
                table: "CompanyProfiles",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonProfiles_Profiles_ProfileId",
                table: "PersonProfiles",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Files_ImageId",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfiles_Profiles_ProfileId",
                table: "CompanyProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonProfiles_Profiles_ProfileId",
                table: "PersonProfiles");

            migrationBuilder.DropIndex(
                name: "IX_PersonProfiles_ProfileId",
                table: "PersonProfiles");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfiles_ProfileId",
                table: "CompanyProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Brands_ImageId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "PersonProfiles");

            migrationBuilder.DropColumn(
                name: "Seo",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "CompanyProfiles");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "Files",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileStream",
                table: "Files",
                newName: "Stream");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Files",
                newName: "Ext");

            migrationBuilder.AddColumn<int>(
                name: "CompanyProfileId",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonProfileId",
                table: "Profiles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CompanyProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_CompanyProfileId",
                table: "Profiles",
                column: "CompanyProfileId",
                unique: true,
                filter: "[CompanyProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_PersonProfileId",
                table: "Profiles",
                column: "PersonProfileId",
                unique: true,
                filter: "[PersonProfileId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_CompanyProfiles_CompanyProfileId",
                table: "Profiles",
                column: "CompanyProfileId",
                principalTable: "CompanyProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_PersonProfiles_PersonProfileId",
                table: "Profiles",
                column: "PersonProfileId",
                principalTable: "PersonProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
