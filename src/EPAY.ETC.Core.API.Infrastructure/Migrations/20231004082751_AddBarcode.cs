using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class AddBarcode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Barcode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ActionCode = table.Column<string>(type: "text", nullable: true),
                    ActionDesc = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Barcode", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Barcode");
        }
    }
}
