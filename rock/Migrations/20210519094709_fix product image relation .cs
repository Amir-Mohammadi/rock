using Microsoft.EntityFrameworkCore.Migrations;
namespace rock.Migrations
{
  public partial class fixproductimagerelation : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddForeignKey(
                 name: "FK_ProductImages_Products_ProductId",
                 table:"ProductImages" , 
                 column: "ProductId",                 
                 principalTable: "Products",
                 principalColumn: "Id",
                 onDelete: ReferentialAction.Restrict);
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}