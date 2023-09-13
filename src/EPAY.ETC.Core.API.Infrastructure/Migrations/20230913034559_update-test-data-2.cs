using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetestdata2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                column: "Amount",
                value: 15000.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                column: "Amount",
                value: 0.0);
        }
    }
}
