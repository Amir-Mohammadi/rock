using Microsoft.EntityFrameworkCore.Migrations;

namespace rock.Migrations
{
    public partial class transportupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Transportation_TransportationId",
                table: "Transports");

            migrationBuilder.RenameColumn(
                name: "TransportationId",
                table: "Transports",
                newName: "ToCityId");

            migrationBuilder.RenameColumn(
                name: "CustomCost",
                table: "Transports",
                newName: "FromCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Transports_TransportationId",
                table: "Transports",
                newName: "IX_Transports_ToCityId");

            migrationBuilder.AddColumn<int>(
                name: "Cost",
                table: "Transports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TrackingCode",
                table: "Transports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_FromCityId",
                table: "Transports",
                column: "FromCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Cities_FromCityId",
                table: "Transports",
                column: "FromCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Cities_ToCityId",
                table: "Transports",
                column: "ToCityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Cities_FromCityId",
                table: "Transports");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Cities_ToCityId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Transports_FromCityId",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Transports");

            migrationBuilder.DropColumn(
                name: "TrackingCode",
                table: "Transports");

            migrationBuilder.RenameColumn(
                name: "ToCityId",
                table: "Transports",
                newName: "TransportationId");

            migrationBuilder.RenameColumn(
                name: "FromCityId",
                table: "Transports",
                newName: "CustomCost");

            migrationBuilder.RenameIndex(
                name: "IX_Transports_ToCityId",
                table: "Transports",
                newName: "IX_Transports_TransportationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Transportation_TransportationId",
                table: "Transports",
                column: "TransportationId",
                principalTable: "Transportation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
