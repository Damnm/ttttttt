using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatefeemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fees_CustomVehicleType_CustomVehicleTypeId",
                table: "Fees");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fees",
                table: "Fees");

            migrationBuilder.RenameTable(
                name: "Fees",
                newName: "Fee");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_TicketId",
                table: "Fee",
                newName: "IX_Fee_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_RFID",
                table: "Fee",
                newName: "IX_Fee_RFID");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_PlateNumber",
                table: "Fee",
                newName: "IX_Fee_PlateNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneOutId",
                table: "Fee",
                newName: "IX_Fee_LaneOutId");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneOutEpoch",
                table: "Fee",
                newName: "IX_Fee_LaneOutEpoch");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneOutDate",
                table: "Fee",
                newName: "IX_Fee_LaneOutDate");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneInId",
                table: "Fee",
                newName: "IX_Fee_LaneInId");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneInEpoch",
                table: "Fee",
                newName: "IX_Fee_LaneInEpoch");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_LaneInDate",
                table: "Fee",
                newName: "IX_Fee_LaneInDate");

            migrationBuilder.RenameIndex(
                name: "IX_Fees_CustomVehicleTypeId",
                table: "Fee",
                newName: "IX_Fee_CustomVehicleTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "TicketTypeId",
                table: "Fee",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TicketId",
                table: "Fee",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "Fee",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "Fee",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "Fee",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Fee",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "Fee",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutVehiclePhotoUrl",
                table: "Fee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutPlateNumberPhotoUrl",
                table: "Fee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutId",
                table: "Fee",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInVehiclePhotoUrl",
                table: "Fee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInPlateNumberPhotoUrl",
                table: "Fee",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInId",
                table: "Fee",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fee",
                table: "Fee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fee_CustomVehicleType_CustomVehicleTypeId",
                table: "Fee",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fee_CustomVehicleType_CustomVehicleTypeId",
                table: "Fee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fee",
                table: "Fee");

            migrationBuilder.RenameTable(
                name: "Fee",
                newName: "Fees");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_TicketId",
                table: "Fees",
                newName: "IX_Fees_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_RFID",
                table: "Fees",
                newName: "IX_Fees_RFID");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_PlateNumber",
                table: "Fees",
                newName: "IX_Fees_PlateNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneOutId",
                table: "Fees",
                newName: "IX_Fees_LaneOutId");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneOutEpoch",
                table: "Fees",
                newName: "IX_Fees_LaneOutEpoch");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneOutDate",
                table: "Fees",
                newName: "IX_Fees_LaneOutDate");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneInId",
                table: "Fees",
                newName: "IX_Fees_LaneInId");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneInEpoch",
                table: "Fees",
                newName: "IX_Fees_LaneInEpoch");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_LaneInDate",
                table: "Fees",
                newName: "IX_Fees_LaneInDate");

            migrationBuilder.RenameIndex(
                name: "IX_Fee_CustomVehicleTypeId",
                table: "Fees",
                newName: "IX_Fees_CustomVehicleTypeId");

            migrationBuilder.AlterColumn<string>(
                name: "TicketTypeId",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TicketId",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "RFID",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateNumber",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PlateColour",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Make",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutVehiclePhotoUrl",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutPlateNumberPhotoUrl",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneOutId",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInVehiclePhotoUrl",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInPlateNumberPhotoUrl",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LaneInId",
                table: "Fees",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fees",
                table: "Fees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fees_CustomVehicleType_CustomVehicleTypeId",
                table: "Fees",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id");
        }
    }
}
