using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppConfig",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsApply = table.Column<bool>(type: "boolean", nullable: false),
                    AppName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    HeaderHeading = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HeaderSubHeading = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HeaderLine1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HeaderLine2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FooterLine1 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FooterLine2 = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppConfig", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppConfig",
                columns: new[] { "Id", "AppName", "CreatedDate", "FooterLine1", "FooterLine2", "HeaderHeading", "HeaderLine1", "HeaderLine2", "HeaderSubHeading", "IsApply" },
                values: new object[] { new Guid("2c0f4a72-0c59-4a76-a379-4be0bc5ebd08"), "Default app config", new DateTime(2023, 9, 27, 13, 55, 24, 173, DateTimeKind.Local).AddTicks(9838), "TP HCM, ", "Người nộp", "Cảng hàng không quốc tế Tân Sơn Nhất", "ĐC: 58 Trường Sơn, Phường 2, Quận Tân Bình, TP. HCM", "ĐT: 123456789 MST: 0312451145112", "CN tổng Công ty hàng không việt - CTCP", true });

            migrationBuilder.CreateIndex(
                name: "IX_AppConfig_IsApply",
                table: "AppConfig",
                column: "IsApply");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppConfig");
        }
    }
}
