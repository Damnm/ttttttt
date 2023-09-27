using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addforeignkeypaymentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Payment_CustomVehicleTypeId",
                table: "Payment",
                column: "CustomVehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_CustomVehicleType_CustomVehicleTypeId",
                table: "Payment",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Fee_FeeId",
                table: "Payment",
                column: "FeeId",
                principalTable: "Fee",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_CustomVehicleType_CustomVehicleTypeId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Fee_FeeId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_CustomVehicleTypeId",
                table: "Payment");
        }
    }
}
