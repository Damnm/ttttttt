using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class correctVehicleCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"));

            migrationBuilder.DeleteData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"));

            migrationBuilder.DeleteData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"));

            migrationBuilder.DeleteData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"));

            migrationBuilder.DeleteData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"));

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "VehicleCategory",
                type: "character varying(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleCategoryType",
                table: "VehicleCategory",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "VehicleCategory");

            migrationBuilder.DropColumn(
                name: "VehicleCategoryType",
                table: "VehicleCategory");

            migrationBuilder.InsertData(
                table: "VehicleCategory",
                columns: new[] { "Id", "CreatedDate", "Desc", "VehicleCategoryName" },
                values: new object[,]
                {
                    { new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe ưu tiên theo tháng" },
                    { new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe nhượng quyền" },
                    { new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe nhượng quyền TCP" },
                    { new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe ưu tiên theo năm" },
                    { new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"), new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xe ưu tiên theo quý" }
                });
        }
    }
}
