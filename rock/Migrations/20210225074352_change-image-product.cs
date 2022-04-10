using System;
using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class changeimageproduct : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Products_Files_PreviewImageId",
          table: "Products");
      migrationBuilder.DropForeignKey(
          name: "FK_Products_ProductCategories_CategoryId",
          table: "Products");
      migrationBuilder.DropIndex(
          name: "IX_Products_PreviewImageId",
          table: "Products");
      migrationBuilder.DropIndex(
          name: "IX_ProductImages_ImageId",
          table: "ProductImages");
      migrationBuilder.DropColumn(
          name: "Seo",
          table: "Files");
      migrationBuilder.RenameColumn(
          name: "CategoryId",
          table: "Products",
          newName: "ProductCategoryId");
      migrationBuilder.RenameIndex(
          name: "IX_Products_CategoryId",
          table: "Products",
          newName: "IX_Products_ProductCategoryId");
      migrationBuilder.AddColumn<string>(
          name: "AltTitle",
          table: "Products",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.AddColumn<int>(
          name: "DefaultProductColorId",
          table: "Products",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<int>(
          name: "PreviewProductImageId",
          table: "Products",
          type: "int",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "ImageAlt",
          table: "ProductImages",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.AddColumn<string>(
          name: "ImageTitle",
          table: "ProductImages",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.AddColumn<string>(
          name: "ImageAlt",
          table: "Brands",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.AddColumn<string>(
          name: "ImageTitle",
          table: "Brands",
          type: "nvarchar(max)",
          nullable: false,
          defaultValue: "");
      migrationBuilder.CreateIndex(
          name: "IX_Products_DefaultProductColorId",
          table: "Products",
          column: "DefaultProductColorId",
          unique: true,
          filter: "[DefaultProductColorId] IS NOT NULL");
      migrationBuilder.CreateIndex(
          name: "IX_Products_PreviewProductImageId",
          table: "Products",
          column: "PreviewProductImageId",
          unique: true,
          filter: "[PreviewProductImageId] IS NOT NULL");
      migrationBuilder.CreateIndex(
          name: "IX_ProductImages_ImageId",
          table: "ProductImages",
          column: "ImageId",
          unique: true);
      migrationBuilder.AddForeignKey(
          name: "FK_Products_ProductCategories_ProductCategoryId",
          table: "Products",
          column: "ProductCategoryId",
          principalTable: "ProductCategories",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Products_ProductColors_DefaultProductColorId",
          table: "Products",
          column: "DefaultProductColorId",
          principalTable: "ProductColors",
          principalColumn: "ColorId",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Products_ProductImages_PreviewProductImageId",
          table: "Products",
          column: "PreviewProductImageId",
          principalTable: "ProductImages",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.Sql("update p set p.PreviewProductImageId = pi.Id from Products p join ProductImages pi on p.PreviewImageId = pi.ImageId");
      migrationBuilder.DropColumn(
        name: "PreviewImageId",
        table: "Products");
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_Products_ProductCategories_ProductCategoryId",
          table: "Products");
      migrationBuilder.DropForeignKey(
          name: "FK_Products_ProductColors_DefaultProductColorId",
          table: "Products");
      migrationBuilder.DropForeignKey(
          name: "FK_Products_ProductImages_PreviewProductImageId",
          table: "Products");
      migrationBuilder.DropIndex(
          name: "IX_Products_DefaultProductColorId",
          table: "Products");
      migrationBuilder.DropIndex(
          name: "IX_Products_PreviewProductImageId",
          table: "Products");
      migrationBuilder.DropIndex(
          name: "IX_ProductImages_ImageId",
          table: "ProductImages");
      migrationBuilder.DropColumn(
          name: "AltTitle",
          table: "Products");
      migrationBuilder.DropColumn(
          name: "DefaultProductColorId",
          table: "Products");
      migrationBuilder.DropColumn(
          name: "PreviewProductImageId",
          table: "Products");
      migrationBuilder.DropColumn(
          name: "ImageAlt",
          table: "ProductImages");
      migrationBuilder.DropColumn(
          name: "ImageTitle",
          table: "ProductImages");
      migrationBuilder.DropColumn(
          name: "ImageAlt",
          table: "Brands");
      migrationBuilder.DropColumn(
          name: "ImageTitle",
          table: "Brands");
      migrationBuilder.RenameColumn(
          name: "ProductCategoryId",
          table: "Products",
          newName: "CategoryId");
      migrationBuilder.RenameIndex(
          name: "IX_Products_ProductCategoryId",
          table: "Products",
          newName: "IX_Products_CategoryId");
      migrationBuilder.AddColumn<Guid>(
          name: "PreviewImageId",
          table: "Products",
          type: "uniqueidentifier",
          nullable: true);
      migrationBuilder.AddColumn<string>(
          name: "Seo",
          table: "Files",
          type: "nvarchar(max)",
          nullable: true);
      migrationBuilder.CreateIndex(
          name: "IX_Products_PreviewImageId",
          table: "Products",
          column: "PreviewImageId");
      migrationBuilder.CreateIndex(
          name: "IX_ProductImages_ImageId",
          table: "ProductImages",
          column: "ImageId");
      migrationBuilder.AddForeignKey(
          name: "FK_Products_Files_PreviewImageId",
          table: "Products",
          column: "PreviewImageId",
          principalTable: "Files",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
      migrationBuilder.AddForeignKey(
          name: "FK_Products_ProductCategories_CategoryId",
          table: "Products",
          column: "CategoryId",
          principalTable: "ProductCategories",
          principalColumn: "Id",
          onDelete: ReferentialAction.Restrict);
    }
  }
}