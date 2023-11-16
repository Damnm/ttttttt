using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addprintlogtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrintLogs",
                columns: table => new
                {
                    PrinterLogId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneOutId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    EmployeeId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PrintType = table.Column<int>(type: "integer", nullable: true),
                    RFID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlateNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    DataJson = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintLogs", x => x.PrinterLogId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintLogs");
        }
    }
}
