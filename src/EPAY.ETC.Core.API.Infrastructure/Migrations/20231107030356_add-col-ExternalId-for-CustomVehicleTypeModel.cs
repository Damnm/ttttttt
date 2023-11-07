using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addcolExternalIdforCustomVehicleTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "CustomVehicleType",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                column: "ExternalId",
                value: "030104");

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                column: "ExternalId",
                value: "030102");

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                column: "ExternalId",
                value: "030103");

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                column: "ExternalId",
                value: "030101");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "CustomVehicleType");
        }
    }
}
