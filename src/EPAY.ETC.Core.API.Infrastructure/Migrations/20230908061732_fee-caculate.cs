using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class feecaculate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomVehicleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomVehicleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Desc = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeBlockFee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomVehicleTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    FromSecond = table.Column<int>(type: "integer", nullable: false),
                    ToSecond = table.Column<int>(type: "integer", nullable: false),
                    BlockDurationInSeconds = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    CustomVehicletypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeBlockFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeBlockFee_CustomVehicleType_CustomVehicleTypeId",
                        column: x => x.CustomVehicleTypeId,
                        principalTable: "CustomVehicleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeVehicleCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleCategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    FeeTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleGroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomVehicleTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    PlateNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    RFID = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IsTCPVehicle = table.Column<bool>(type: "boolean", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeVehicleCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeVehicleCategory_CustomVehicleType_CustomVehicleTypeId",
                        column: x => x.CustomVehicleTypeId,
                        principalTable: "CustomVehicleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeVehicleCategory_FeeType_FeeTypeId",
                        column: x => x.FeeTypeId,
                        principalTable: "FeeType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeVehicleCategory_VehicleCategory_VehicleCategoryId",
                        column: x => x.VehicleCategoryId,
                        principalTable: "VehicleCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeVehicleCategory_VehicleGroup_VehicleGroupId",
                        column: x => x.VehicleGroupId,
                        principalTable: "VehicleGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomVehicleType",
                columns: new[] { "Id", "CreatedDate", "Desc", "Name" },
                values: new object[,]
                {
                    { new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3786), "Xe loại 4", "Type4" },
                    { new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3679), "Xe loại 2", "Type2" },
                    { new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3740), "Xe loại 3", "Type3" },
                    { new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(3400), "Xe loại 1", "Type1" }
                });

            migrationBuilder.InsertData(
                table: "FeeType",
                columns: new[] { "Id", "Amount", "CreatedDate", "Desc", "Name" },
                values: new object[,]
                {
                    { new Guid("04595036-c8a8-4800-9513-c4015b98da3b"), null, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4727), "Tính phí theo ngày", "DayBlock" },
                    { new Guid("1143d8c3-22e2-4bd5-a690-89ca0c47b3c9"), null, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4697), "Tính phí theo thời gian", "TimeBlock" },
                    { new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"), 0f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4533), "Miễn phí", "Free" },
                    { new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"), 0f, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4661), "Phí cố định", "Fixed" }
                });

            migrationBuilder.InsertData(
                table: "VehicleCategory",
                columns: new[] { "Id", "CreatedDate", "Desc", "Name" },
                values: new object[,]
                {
                    { new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4193), null, "Xe ưu tiên theo tháng" },
                    { new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4189), null, "Xe nhượng quyền" },
                    { new Guid("82f143d3-b2ed-40d6-a59e-4fc980a24450"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4192), null, "Xe nhượng quyền TCP" },
                    { new Guid("ac9b71a5-0541-4d2e-a358-6afac6d6c525"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4195), null, "Xe ưu tiên theo năm" },
                    { new Guid("bd4e670d-8cae-46fa-8bac-d77ac139a044"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4194), null, "Xe ưu tiên theo quý" }
                });

            migrationBuilder.InsertData(
                table: "VehicleGroup",
                columns: new[] { "Id", "CreatedDate", "Desc", "Name" },
                values: new object[,]
                {
                    { new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4986), null, "Taxi Xanh" },
                    { new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4987), null, "Công ty vận tải hành khách" },
                    { new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3"), new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(4983), null, "Taxi Mai Linh" }
                });

            migrationBuilder.InsertData(
                table: "FeeVehicleCategory",
                columns: new[] { "Id", "CreatedDate", "CustomVehicleTypeId", "FeeTypeId", "IsTCPVehicle", "PlateNumber", "RFID", "ValidFrom", "ValidTo", "VehicleCategoryId", "VehicleGroupId" },
                values: new object[,]
                {
                    { new Guid("1d6603bb-d361-4111-aa45-e780f50b6974"), new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7745), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"), false, "50A3008", null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"), new Guid("1fc5fc58-94e4-4169-a576-3cd9ecf8eb96") },
                    { new Guid("a15041a9-1d57-4ae3-b070-2d96aaa041ec"), new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7719), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), new Guid("46b26ea4-abfd-4b9f-bdf4-ec0e434d9ffc"), false, "51A3268", null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"), new Guid("efbe78bc-290b-4a01-a596-bbc62f60f5f3") },
                    { new Guid("a743e3e1-d6aa-49c5-a63f-28ba262bc2b8"), new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7753), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"), false, "51A0968", null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), new Guid("70884a61-39f3-4e8e-b936-d5b18652d3ac"), new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d") },
                    { new Guid("b780afae-6c9e-4730-a054-8ab8a876dffe"), new DateTime(2023, 9, 8, 6, 17, 31, 931, DateTimeKind.Utc).AddTicks(7759), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), new Guid("30ee8597-aa3e-43e7-a1f1-559ee2d4b85e"), false, "29A3268", null, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), new Guid("2b0557d0-cc6b-4fc2-a0b3-08788c9fd8c7"), new Guid("ec2a686b-8adc-4053-9e2e-4942cab0168d") }
                });

            migrationBuilder.InsertData(
                table: "TimeBlockFee",
                columns: new[] { "Id", "Amount", "BlockDurationInSeconds", "CreatedDate", "CustomVehicleTypeId", "CustomVehicletypeId", "FromSecond", "ToSecond" },
                values: new object[,]
                {
                    { new Guid("0c8d860a-c5ba-473c-a3f6-95aafd295a70"), 21000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8275), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null, 3600, 5399 },
                    { new Guid("120fd104-b6e4-403f-87d7-811ccb1c61e4"), 28000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8283), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null, 3600, 5399 },
                    { new Guid("2e8ab3f8-8d72-4f42-831f-b0100f814a23"), 14000f, 3000, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8273), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null, 600, 3599 },
                    { new Guid("3ae0b8be-525b-4ee4-9d49-d2889c6998c3"), 52000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8306), new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null, 5400, 7199 },
                    { new Guid("8585a134-fa8f-467e-8e66-f37e75444a65"), 24000f, 3000, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8289), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null, 600, 3599 },
                    { new Guid("8a8040ae-479c-4824-a0c2-3b4277d0ea9c"), 24000f, 3000, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8301), new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null, 600, 3599 },
                    { new Guid("8b000abd-8e74-47a3-8a90-299dc37fac4d"), 14000f, 600, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8287), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null, 0, 599 },
                    { new Guid("9302c9e0-12c2-437c-bd2d-92ed4c159e9f"), 37000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8285), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null, 5400, 7199 },
                    { new Guid("9e891bfc-2f03-4382-8b7e-6306c2757963"), 52000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8293), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null, 5400, 7199 },
                    { new Guid("ad21057b-6071-4e56-8949-ce60bf54f75b"), 9000f, 600, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8267), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null, 0, 599 },
                    { new Guid("adda0b07-7bd5-470b-9f89-77bb6b5cbfb2"), 38000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8303), new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null, 3600, 5399 },
                    { new Guid("b3b643cb-488d-48f3-a167-ea9531db75ca"), 38000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8291), new Guid("be652877-ca81-4fb4-bfa1-b9cec61f9e6b"), null, 3600, 5399 },
                    { new Guid("df059c09-28aa-4134-919a-e3b3041213a4"), 19000f, 3000, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8281), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null, 600, 3599 },
                    { new Guid("e3772040-a29c-4a40-bd65-17d8be7211bb"), 28000f, 1800, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8277), new Guid("fffbf4d1-8b76-4f3a-9070-0cfa0a658f08"), null, 5400, 7199 },
                    { new Guid("f8d3b541-2f77-4f14-bbe6-8a3028fccd07"), 14000f, 600, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8279), new Guid("a4a39e55-85c0-4761-ba64-f941111186f9"), null, 0, 599 },
                    { new Guid("f8d9683b-7cb4-4ce6-985c-5aa0a8f944e0"), 24000f, 600, new DateTime(2023, 9, 8, 6, 17, 31, 930, DateTimeKind.Utc).AddTicks(8295), new Guid("090a7db5-2d5d-4c1c-a32c-27f946f8dd61"), null, 0, 599 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_CustomVehicleTypeId",
                table: "FeeVehicleCategory",
                column: "CustomVehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_FeeTypeId",
                table: "FeeVehicleCategory",
                column: "FeeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_PlateNumber",
                table: "FeeVehicleCategory",
                column: "PlateNumber");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_RFID",
                table: "FeeVehicleCategory",
                column: "RFID");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_VehicleCategoryId",
                table: "FeeVehicleCategory",
                column: "VehicleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeVehicleCategory_VehicleGroupId",
                table: "FeeVehicleCategory",
                column: "VehicleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlockFee_CustomVehicleTypeId",
                table: "TimeBlockFee",
                column: "CustomVehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeBlockFee_FromSecond_ToSecond",
                table: "TimeBlockFee",
                columns: new[] { "FromSecond", "ToSecond" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeVehicleCategory");

            migrationBuilder.DropTable(
                name: "TimeBlockFee");

            migrationBuilder.DropTable(
                name: "FeeType");

            migrationBuilder.DropTable(
                name: "VehicleCategory");

            migrationBuilder.DropTable(
                name: "VehicleGroup");

            migrationBuilder.DropTable(
                name: "CustomVehicleType");
        }
    }
}
