using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addorderTimeBlockFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFee");

            migrationBuilder.DropIndex(
                name: "IX_TimeBlockFee_CustomVehicleTypeId",
                table: "TimeBlockFee");

            migrationBuilder.DropColumn(
                name: "CustomVehicleTypeId",
                table: "TimeBlockFee");

            migrationBuilder.RenameColumn(
                name: "CustomVehicletypeId",
                table: "TimeBlockFee",
                newName: "CustomVehicleTypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomVehicleTypeId",
                table: "TimeBlockFee",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "TimeBlockFee",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), 2 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), 2 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), 1 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), 3 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), 1 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), 1 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), 0 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), 3 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), 3 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), 0 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), 2 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), 2 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("df059c09-28aa-4134-919a-e3b3041213a4"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), 1 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), 3 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), 0 });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"),
                columns: new[] { "CustomVehicleTypeId", "Order" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlockFee_CustomVehicleTypeId",
                table: "TimeBlockFee",
                column: "CustomVehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFee",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFee");

            migrationBuilder.DropIndex(
                name: "IX_TimeBlockFee_CustomVehicleTypeId",
                table: "TimeBlockFee");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "TimeBlockFee");

            migrationBuilder.RenameColumn(
                name: "CustomVehicleTypeId",
                table: "TimeBlockFee",
                newName: "CustomVehicletypeId");

            migrationBuilder.AlterColumn<Guid>(
                name: "CustomVehicletypeId",
                table: "TimeBlockFee",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CustomVehicleTypeId",
                table: "TimeBlockFee",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("df059c09-28aa-4134-919a-e3b3041213a4"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null });

            migrationBuilder.UpdateData(
                table: "TimeBlockFee",
                keyColumn: "Id",
                keyValue: new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"),
                columns: new[] { "CustomVehicleTypeId", "CustomVehicletypeId" },
                values: new object[] { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null });

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlockFee_CustomVehicleTypeId",
                table: "TimeBlockFee",
                column: "CustomVehicleTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId",
                table: "TimeBlockFee",
                column: "CustomVehicleTypeId",
                principalTable: "CustomVehicleType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
