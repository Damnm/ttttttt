using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class BarcodeModelAddBarcodeAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticatedEmployees");

            migrationBuilder.AlterColumn<string>(
                name: "ActionDesc",
                table: "Barcode",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActionCode",
                table: "Barcode",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BarcodeAction",
                table: "Barcode",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Barcode",
                columns: new[] { "Id", "ActionCode", "ActionDesc", "BarcodeAction", "CreatedDate", "EmployeeId" },
                values: new object[,]
                {
                    { new Guid("224874bf-0b78-41f5-a827-7df9f3ae2412"), "W6FDEZ", "Barcode đăng nhập cho nhân viên", "ControlUIAccess", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), "030001" },
                    { new Guid("6458f220-28e0-4d6b-9367-be6f5b6f2f2f"), "K6GRG7", "Barcode điều khiển barrier", "ControlBarrier", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), "030001" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Barcode",
                keyColumn: "Id",
                keyValue: new Guid("224874bf-0b78-41f5-a827-7df9f3ae2412"));

            migrationBuilder.DeleteData(
                table: "Barcode",
                keyColumn: "Id",
                keyValue: new Guid("6458f220-28e0-4d6b-9367-be6f5b6f2f2f"));

            migrationBuilder.DropColumn(
                name: "BarcodeAction",
                table: "Barcode");

            migrationBuilder.AlterColumn<string>(
                name: "ActionDesc",
                table: "Barcode",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ActionCode",
                table: "Barcode",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AuthenticatedEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    EmployeeId = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    JwtToken = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticatedEmployees", x => x.Id);
                });
        }
    }
}
