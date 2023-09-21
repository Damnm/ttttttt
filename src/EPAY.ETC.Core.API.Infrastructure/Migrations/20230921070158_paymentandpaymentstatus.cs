using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class paymentandpaymentstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Epoch",
                table: "Fusions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneInId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    FeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    LaneOutId = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    RFID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Make = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    Model = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    PlateNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    VehicleTypeId = table.Column<string>(type: "text", nullable: true),
                    CustomVehicleTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatus",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    PaymentReferenceId = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentStatus_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_FeeId",
                table: "Payment",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_LaneInId",
                table: "Payment",
                column: "LaneInId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_LaneOutId",
                table: "Payment",
                column: "LaneOutId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PlateNumber",
                table: "Payment",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_RFID",
                table: "Payment",
                column: "RFID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatus_PaymentId",
                table: "PaymentStatus",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatus_PaymentReferenceId",
                table: "PaymentStatus",
                column: "PaymentReferenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentStatus");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.AlterColumn<float>(
                name: "Epoch",
                table: "Fusions",
                type: "real",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
