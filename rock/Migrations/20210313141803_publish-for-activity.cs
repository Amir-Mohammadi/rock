using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class publishforactivity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PublishAt",
                table: "ThreadActivities",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublisherId",
                table: "ThreadActivities",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThreadActivities_PublisherId",
                table: "ThreadActivities",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ColorId",
                table: "CartItems",
                column: "ColorId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Colors_ColorId",
                table: "CartItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ThreadActivities_Users_PublisherId",
                table: "ThreadActivities",
                column: "PublisherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Colors_ColorId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ThreadActivities_Users_PublisherId",
                table: "ThreadActivities");

            migrationBuilder.DropIndex(
                name: "IX_ThreadActivities_PublisherId",
                table: "ThreadActivities");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ColorId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PublishAt",
                table: "ThreadActivities");

            migrationBuilder.DropColumn(
                name: "PublisherId",
                table: "ThreadActivities");
        }
    }
}
