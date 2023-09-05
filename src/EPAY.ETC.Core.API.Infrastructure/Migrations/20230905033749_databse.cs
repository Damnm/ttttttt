using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class databse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fusions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Epoch = table.Column<float>(type: "real", nullable: false),
                    Loop1 = table.Column<bool>(type: "boolean", nullable: false),
                    RFID = table.Column<bool>(type: "boolean", nullable: false),
                    Cam1 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Loop2 = table.Column<bool>(type: "boolean", nullable: false),
                    Cam2 = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    Loop3 = table.Column<bool>(type: "boolean", nullable: false),
                    ReversedLoop1 = table.Column<bool>(type: "boolean", nullable: false),
                    ReversedLoop2 = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fusions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fusions");
        }
    }
}
