using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class updateManualBarrierControlModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 13, 50, 2, 906, DateTimeKind.Local).AddTicks(5983));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 28, 13, 25, 27, 117, DateTimeKind.Local).AddTicks(929));
        }
    }
}
