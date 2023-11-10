using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addticketTypetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "TicketTypeId",
                table: "Fee",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShiftId",
                table: "Fee",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "TicketType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TicketType",
                columns: new[] { "Id", "Code", "CreatedDate", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), "Priority", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe ưu tiên" },
                    { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), "FreeEntry", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xả trạm" },
                    { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), "OneTimePass", new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe vé lượt" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fee_TicketTypeId",
                table: "Fee",
                column: "TicketTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fee_TicketType_TicketTypeId",
                table: "Fee",
                column: "TicketTypeId",
                principalTable: "TicketType",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fee_TicketType_TicketTypeId",
                table: "Fee");

            migrationBuilder.DropTable(
                name: "TicketType");

            migrationBuilder.DropIndex(
                name: "IX_Fee_TicketTypeId",
                table: "Fee");

            migrationBuilder.AlterColumn<string>(
                name: "TicketTypeId",
                table: "Fee",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ShiftId",
                table: "Fee",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);
        }
    }
}
