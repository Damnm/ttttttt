using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class Updatemaxlengthentities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFeeFormulas");

            migrationBuilder.DropTable(
                name: "VehiclePaymentTransactions");

            migrationBuilder.DropTable(
                name: "VehicleTransactionModels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBlockFeeFormulas",
                table: "TimeBlockFeeFormulas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaneInRFIDTransactionLogs",
                table: "LaneInRFIDTransactionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaneInCameraTransactionLogs",
                table: "LaneInCameraTransactionLogs");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vehicle");

            migrationBuilder.RenameTable(
                name: "TimeBlockFeeFormulas",
                newName: "TimeBlockFeeFormula");

            migrationBuilder.RenameTable(
                name: "LaneInRFIDTransactionLogs",
                newName: "LaneInRFIDTransactionLog");

            migrationBuilder.RenameTable(
                name: "LaneInCameraTransactionLogs",
                newName: "LaneInCameraTransactionLog");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBlockFeeFormulas_CustomVehicleTypeId",
                table: "TimeBlockFeeFormula",
                newName: "IX_TimeBlockFeeFormula_CustomVehicleTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleTypeId",
                table: "Payment",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutId",
                table: "ManualBarrierControl",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ManualBarrierControl",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "Vehicle",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "Vehicle",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "Vehicle",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColor",
                table: "Vehicle",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Vehicle",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "Vehicle",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePhotoUrl",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFIDReaderMacAddr",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFIDReaderIPAddr",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumberPhotoUrl",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "LaneInCameraTransactionLog",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePhotoUrl",
                table: "LaneInCameraTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "LaneInCameraTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumberPhotoUrl",
                table: "LaneInCameraTransactionLog",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "LaneInCameraTransactionLog",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "LaneInCameraTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "LaneInCameraTransactionLog",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "LaneInCameraTransactionLog",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraReaderMacAddr",
                table: "LaneInCameraTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraReaderIPAddr",
                table: "LaneInCameraTransactionLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBlockFeeFormula",
                table: "TimeBlockFeeFormula",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaneInRFIDTransactionLog",
                table: "LaneInRFIDTransactionLog",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaneInCameraTransactionLog",
                table: "LaneInCameraTransactionLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBlockFeeFormula_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFeeFormula",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBlockFeeFormula_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFeeFormula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeBlockFeeFormula",
                table: "TimeBlockFeeFormula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaneInRFIDTransactionLog",
                table: "LaneInRFIDTransactionLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LaneInCameraTransactionLog",
                table: "LaneInCameraTransactionLog");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "TimeBlockFeeFormula",
                newName: "TimeBlockFeeFormulas");

            migrationBuilder.RenameTable(
                name: "LaneInRFIDTransactionLog",
                newName: "LaneInRFIDTransactionLogs");

            migrationBuilder.RenameTable(
                name: "LaneInCameraTransactionLog",
                newName: "LaneInCameraTransactionLogs");

            migrationBuilder.RenameIndex(
                name: "IX_TimeBlockFeeFormula_CustomVehicleTypeId",
                table: "TimeBlockFeeFormulas",
                newName: "IX_TimeBlockFeeFormulas_CustomVehicleTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleTypeId",
                table: "Payment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutId",
                table: "ManualBarrierControl",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ManualBarrierControl",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColor",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePhotoUrl",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFIDReaderMacAddr",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFIDReaderIPAddr",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumberPhotoUrl",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "LaneInRFIDTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehiclePhotoUrl",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumberPhotoUrl",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraReaderMacAddr",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CameraReaderIPAddr",
                table: "LaneInCameraTransactionLogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeBlockFeeFormulas",
                table: "TimeBlockFeeFormulas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaneInRFIDTransactionLogs",
                table: "LaneInRFIDTransactionLogs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LaneInCameraTransactionLogs",
                table: "LaneInCameraTransactionLogs",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "VehiclePaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsSuccessful = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentType = table.Column<string>(type: "text", nullable: true),
                    VehicleTransactionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehiclePaymentTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTransactionModels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    ExternalEmployeeId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneInDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LaneInId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneInPlateNumberPhotoURL = table.Column<string>(type: "text", nullable: true),
                    LaneInVehiclePhotoURL = table.Column<string>(type: "text", nullable: true),
                    LaneOutDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LaneOutId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneOutPlateNumberPhotoURL = table.Column<string>(type: "text", nullable: true),
                    LaneOutVehiclePhotoURL = table.Column<string>(type: "text", nullable: true),
                    PaymentMethod = table.Column<string>(type: "text", nullable: true),
                    PlateNumber = table.Column<string>(type: "text", nullable: true),
                    RFID = table.Column<string>(type: "text", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTransactionModels", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFeeFormulas",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
