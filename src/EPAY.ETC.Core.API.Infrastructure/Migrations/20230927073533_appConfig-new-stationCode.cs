using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class appConfignewstationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StationCode",
                table: "AppConfig",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                columns: new[] { "CreatedDate", "StationCode" },
                values: new object[] { new DateTime(2023, 9, 27, 7, 34, 46, 0, DateTimeKind.Unspecified), "03" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationCode",
                table: "AppConfig");

            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 27, 13, 55, 24, 173, DateTimeKind.Local).AddTicks(9838));
        }
    }
}
