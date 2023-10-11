using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class updateentitystructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VehicleGroup",
                newName: "GroupName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "VehicleCategory",
                newName: "VehicleCategoryName");

            migrationBuilder.RenameColumn(
                name: "VehicleTypeId",
                table: "Payment",
                newName: "VehicleType");

            migrationBuilder.RenameColumn(
                name: "CameraReaderMacAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CamerraIPAddr");

            migrationBuilder.RenameColumn(
                name: "CameraReaderIPAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CameraMacAddr");

            migrationBuilder.RenameColumn(
                name: "Cam2",
                table: "Fusion",
                newName: "CCTVCam2");

            migrationBuilder.RenameColumn(
                name: "Cam1",
                table: "Fusion",
                newName: "ANPRCam1");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FeeType",
                newName: "FeeName");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "ManualBarrierControl",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "LaneInRFIDTransactionLog",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "Seat",
                table: "LaneInRFIDTransactionLog",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "LaneInId",
                table: "LaneInRFIDTransactionLog",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "Epoch",
                table: "LaneInRFIDTransactionLog",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<string>(
                name: "LaneInId",
                table: "LaneInCameraTransactionLog",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "Epoch",
                table: "LaneInCameraTransactionLog",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "VehicleGroup",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VehicleCategoryName",
                table: "VehicleCategory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VehicleType",
                table: "Payment",
                newName: "VehicleTypeId");

            migrationBuilder.RenameColumn(
                name: "CamerraIPAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CameraReaderMacAddr");

            migrationBuilder.RenameColumn(
                name: "CameraMacAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CameraReaderIPAddr");

            migrationBuilder.RenameColumn(
                name: "CCTVCam2",
                table: "Fusion",
                newName: "Cam2");

            migrationBuilder.RenameColumn(
                name: "ANPRCam1",
                table: "Fusion",
                newName: "Cam1");

            migrationBuilder.RenameColumn(
                name: "FeeName",
                table: "FeeType",
                newName: "Name");

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "ManualBarrierControl",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Weight",
                table: "LaneInRFIDTransactionLog",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Seat",
                table: "LaneInRFIDTransactionLog",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LaneInId",
                table: "LaneInRFIDTransactionLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Epoch",
                table: "LaneInRFIDTransactionLog",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "LaneInId",
                table: "LaneInCameraTransactionLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Epoch",
                table: "LaneInCameraTransactionLog",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
