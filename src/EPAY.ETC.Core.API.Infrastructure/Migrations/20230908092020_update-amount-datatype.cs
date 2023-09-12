using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateamountdatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ToSecond",
                table: "TimeBlockFee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "FromSecond",
                table: "TimeBlockFee",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "TimeBlockFee",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "FeeType",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("04595036-c8a8-4800-9513-c4015b98da3b"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { null, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { null, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { 0.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { 0.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a743e3e1-d6aa-49c5-a63f-28ba262bc2b8"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("b780afae-6c9e-4730-a054-8ab8a876dffe"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 21000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3600L, 5399L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 28000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3600L, 5399L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 600L, 3599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 52000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5400L, 7199L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 600L, 3599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 600L, 3599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 37000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5400L, 7199L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 52000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5400L, 7199L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 9000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 38000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3600L, 5399L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 38000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3600L, 5399L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("df059c09-28aa-4134-919a-e3b3041213a4"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 19000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 600L, 3599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 28000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 5400L, 7199L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 599L });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000.0, new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc), 0L, 599L });

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ToSecond",
                table: "TimeBlockFee",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "FromSecond",
                table: "TimeBlockFee",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "TimeBlockFee",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "FeeType",
                type: "real",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3740));

            migrationBuilder.UpdateData(
                table: "CustomVehicleType",
                keyColumn: "Id",
                keyValue: new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("04595036-c8a8-4800-9513-c4015b98da3b"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { null, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4727) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { null, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4697) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { 0f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4533) });

            migrationBuilder.UpdateData(
                table: "FeeType",
                keyColumn: "Id",
                keyValue: new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"),
                columns: new[] { "Amount", "CreatedDate" },
                values: new object[] { 0f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4661) });

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7745));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7719));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("a743e3e1-d6aa-49c5-a63f-28ba262bc2b8"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7753));

            migrationBuilder.UpdateData(
                table: "FeeVehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("b780afae-6c9e-4730-a054-8ab8a876dffe"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 21000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8275), 3600, 5399 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 28000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8283), 3600, 5399 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8273), 600, 3599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 52000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8306), 5400, 7199 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8289), 600, 3599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8301), 600, 3599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8287), 0, 599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 37000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8285), 5400, 7199 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 52000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8293), 5400, 7199 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 9000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8267), 0, 599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 38000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8303), 3600, 5399 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 38000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8291), 3600, 5399 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("df059c09-28aa-4134-919a-e3b3041213a4"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 19000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8281), 600, 3599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 28000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8277), 5400, 7199 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 14000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8279), 0, 599 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"),
                columns: new[] { "Amount", "CreatedDate", "FromSecond", "ToSecond" },
                values: new object[] { 24000f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8295), 0, 599 });

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4193));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4189));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4192));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4195));

            migrationBuilder.UpdateData(
                table: "VehicleCategory",
                keyColumn: "Id",
                keyValue: new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4194));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4986));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4987));

            migrationBuilder.UpdateData(
                table: "VehicleGroup",
                keyColumn: "Id",
                keyValue: new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3"),
                column: "CreatedDate",
                value: new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4983));
        }
    }
}
