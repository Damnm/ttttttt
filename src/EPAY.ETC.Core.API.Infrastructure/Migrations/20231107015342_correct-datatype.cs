using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class correctdatatype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ShiftId",
                table: "Fee",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Function",
                table: "ErrorResponse",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpayCode",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("1105a445-09cd-48f2-97f9-1cc6b9be7672"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "400", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "301" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("1432aea7-f727-4282-9707-23dfbe417d53"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "302" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25cf2789-c3f8-48c1-9392-920b3ea5a0a4"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "400", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "301" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333022"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "305" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("2c1ad42f-9c67-4ed3-a2f1-f8b912acc396"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "400", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "301" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("378a72d0-999e-49e9-bab3-9f68bb591de9"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "306" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("7fe27592-d680-41a0-a8a6-0ea9441495a0"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "400", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "301" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b0"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "308" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d64"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "2", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "201" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("e09d4996-85ba-49db-b773-f0ea32590abd"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "27", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "226" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a62f"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "1", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "501" });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("fdf50f73-ede5-4db2-82e1-5e0aa08b6c0e"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { "13", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "513" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ShiftId",
                table: "Fee",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Function",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EpayCode",
                table: "ErrorResponse",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "ErrorResponse",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("1105a445-09cd-48f2-97f9-1cc6b9be7672"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 400, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4124), 301 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("1432aea7-f727-4282-9707-23dfbe417d53"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 500, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4134), 302 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25cf2789-c3f8-48c1-9392-920b3ea5a0a4"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 400, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4136), 301 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333022"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 500, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4138), 305 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("2c1ad42f-9c67-4ed3-a2f1-f8b912acc396"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 400, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4140), 301 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("378a72d0-999e-49e9-bab3-9f68bb591de9"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 500, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4141), 306 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("7fe27592-d680-41a0-a8a6-0ea9441495a0"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 400, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4143), 301 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b0"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 500, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4145), 308 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d64"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 2, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4146), 201 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("e09d4996-85ba-49db-b773-f0ea32590abd"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 27, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4194), 226 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a62f"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 1, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4196), 501 });

            migrationBuilder.UpdateData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("fdf50f73-ede5-4db2-82e1-5e0aa08b6c0e"),
                columns: new[] { "Code", "CreatedDate", "EpayCode" },
                values: new object[] { 13, new DateTime(2023, 11, 2, 16, 44, 22, 864, DateTimeKind.Local).AddTicks(4198), 513 });
        }
    }
}
