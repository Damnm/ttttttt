using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class updateAuthenticatedEmployeeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticatedEmployees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EmployeeId = table.Column<string>(type: "text", nullable: true),
                    Username = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    JwtToken = table.Column<string>(type: "text", nullable: true),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticatedEmployees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticatedEmployees");
        }
    }
}
