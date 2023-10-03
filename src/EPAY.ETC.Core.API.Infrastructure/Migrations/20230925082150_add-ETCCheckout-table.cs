using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addETCCheckouttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ETCCheckout",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceProvider = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    RFID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    PlateNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETCCheckout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETCCheckout_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETCCheckout_PaymentId",
                table: "ETCCheckout",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_ETCCheckout_TransactionId",
                table: "ETCCheckout",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ETCCheckout_TransactionId_RFID_PlateNumber",
                table: "ETCCheckout",
                columns: new[] { "TransactionId", "RFID", "PlateNumber" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETCCheckout");
        }
    }
}
