using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addfeildLaneInCameraTransactionLogtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlateNumberRearPhotoUrl",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearPlateColour",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearPlateNumber",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRearPhotoUrl",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlateNumberRearPhotoUrl",
                table: "LaneInCameraTransactionLog",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearPlateColour",
                table: "LaneInCameraTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RearPlateNumber",
                table: "LaneInCameraTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRearPhotoUrl",
                table: "LaneInCameraTransactionLog",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateNumberRearPhotoUrl",
                table: "LaneInRFIDTransactionLog");

            migrationBuilder.DropColumn(
                name: "RearPlateColour",
                table: "LaneInRFIDTransactionLog");

            migrationBuilder.DropColumn(
                name: "RearPlateNumber",
                table: "LaneInRFIDTransactionLog");

            migrationBuilder.DropColumn(
                name: "VehicleRearPhotoUrl",
                table: "LaneInRFIDTransactionLog");

            migrationBuilder.DropColumn(
                name: "PlateNumberRearPhotoUrl",
                table: "LaneInCameraTransactionLog");

            migrationBuilder.DropColumn(
                name: "RearPlateColour",
                table: "LaneInCameraTransactionLog");

            migrationBuilder.DropColumn(
                name: "RearPlateNumber",
                table: "LaneInCameraTransactionLog");

            migrationBuilder.DropColumn(
                name: "VehicleRearPhotoUrl",
                table: "LaneInCameraTransactionLog");
        }
    }
}
