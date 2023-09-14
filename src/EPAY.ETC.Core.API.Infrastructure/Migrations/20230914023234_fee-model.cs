using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ObjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    LaneInId = table.Column<string>(type: "text", nullable: true),
                    LaneInDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LaneInEpoch = table.Column<long>(type: "bigint", nullable: true),
                    LaneOutId = table.Column<string>(type: "text", nullable: true),
                    LaneOutDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LaneOutEpoch = table.Column<long>(type: "bigint", nullable: true),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    RFID = table.Column<string>(type: "text", nullable: true),
                    Make = table.Column<string>(type: "text", nullable: true),
                    Model = table.Column<string>(type: "text", nullable: true),
                    PlateNumber = table.Column<string>(type: "text", nullable: true),
                    PlateColour = table.Column<string>(type: "text", nullable: true),
                    CustomVehicleTypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    Seat = table.Column<int>(type: "integer", nullable: true),
                    Weight = table.Column<int>(type: "integer", nullable: true),
                    LaneInPlateNumberPhotoUrl = table.Column<string>(type: "text", nullable: true),
                    LaneInVehiclePhotoUrl = table.Column<string>(type: "text", nullable: true),
                    LaneOutPlateNumberPhotoUrl = table.Column<string>(type: "text", nullable: true),
                    LaneOutVehiclePhotoUrl = table.Column<string>(type: "text", nullable: true),
                    ConfidenceScore = table.Column<float>(type: "real", nullable: true),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    VehicleCategoryId = table.Column<Guid>(type: "uuid", nullable: true),
                    TicketTypeId = table.Column<string>(type: "text", nullable: true),
                    TicketId = table.Column<string>(type: "text", nullable: true),
                    ShiftId = table.Column<Guid>(type: "uuid", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fees_CustomVehicleType_CustomVehicleTypeId",
                        column: x => x.CustomVehicleTypeId,
                        principalTable: "CustomVehicleType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fees_CustomVehicleTypeId",
                table: "Fees",
                column: "CustomVehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneInDate",
                table: "Fees",
                column: "LaneInDate");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneInEpoch",
                table: "Fees",
                column: "LaneInEpoch");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneInId",
                table: "Fees",
                column: "LaneInId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneOutDate",
                table: "Fees",
                column: "LaneOutDate");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneOutEpoch",
                table: "Fees",
                column: "LaneOutEpoch");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_LaneOutId",
                table: "Fees",
                column: "LaneOutId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_PlateNumber",
                table: "Fees",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_RFID",
                table: "Fees",
                column: "RFID");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_TicketId",
                table: "Fees",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fees");
        }
    }
}
