using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatetestdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"),
                column: "RFID",
                value: "840326156843215625");

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"),
                column: "RFID",
                value: "843206065135832015");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"),
                column: "RFID",
                value: null);

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"),
                column: "RFID",
                value: null);
        }
    }
}
