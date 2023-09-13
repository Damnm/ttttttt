using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TimeBlockFeeFormula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Order",
                table: "TimeBlockFee",
                newName: "BlockNumber");

            migrationBuilder.CreateTable(
                name: "TimeBlockFeeFormulas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomVehicleTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromBlockNumber = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    IntervalInSeconds = table.Column<long>(type: "bigint", nullable: false),
                    ApplyDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBlockFeeFormulas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeBlockFeeFormulas_CustomVehicleType_CustomVehicleTypeId",
                        column: x => x.CustomVehicleTypeId,
                        principalTable: "CustomVehicleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TimeBlockFeeFormulas",
                columns: new[] { "Id", "Amount", "ApplyDate", "CreatedDate", "CustomVehicleTypeId", "FromBlockNumber", "IntervalInSeconds" },
                values: new object[,]
                {
                    { new Guid("41369f70-ab4d-4199-a1b3-f7746fa0ff88"), 14000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), 2, 1800L },
                    { new Guid("667b13b4-088e-4a1a-bd36-ec15e795109b"), 7000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), 2, 1800L },
                    { new Guid("8376b7a6-4330-4133-9e47-afd0d3f7c921"), 14000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), 2, 1800L },
                    { new Guid("98c39b48-1248-4471-ae72-22e51e456307"), 9000.0, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), 2, 1800L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlockFeeFormulas_CustomVehicleTypeId",
                table: "TimeBlockFeeFormulas",
                column: "CustomVehicleTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeBlockFeeFormulas");

            migrationBuilder.RenameColumn(
                name: "BlockNumber",
                table: "TimeBlockFee",
                newName: "Order");
        }
    }
}
