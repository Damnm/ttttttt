using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addtablemanualBarrier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManualBarrierControl",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Action = table.Column<int>(type: "integer", nullable: false),
                    LaneOutId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualBarrierControl", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                columns: new[] { "CreatedDate", "StationCode" },
                values: new object[] { new DateTime(2023, 9, 27, 7, 34, 46, 0, DateTimeKind.Unspecified), "03" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualBarrierControl");

            migrationBuilder.UpdateData(
                table: "AppConfig",
                keyColumn: "Id",
                keyValue: new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"),
                columns: new[] { "CreatedDate", "StationCode" },
                values: new object[] { new DateTime(2023, 9, 28, 13, 57, 53, 516, DateTimeKind.Local).AddTicks(5147), null });
        }
    }
}
