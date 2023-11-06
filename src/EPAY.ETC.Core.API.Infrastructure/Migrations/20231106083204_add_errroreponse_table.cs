using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class add_errroreponse_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Source = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Function = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    ErrorCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EpayCode = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    EpayMessage = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorResponse", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ErrorResponse",
                columns: new[] { "Id", "Code", "CreatedDate", "EpayCode", "EpayMessage", "ErrorCode", "Function", "Source", "Status" },
                values: new object[,]
                {
                    { new Guid("1105a445-09cd-48f2-97f9-1cc6b9be7672"), "400", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4402), "301", "Thiếu tham số đầu vào", "", "Checkin", "VETC", "BAD_REQUEST" },
                    { new Guid("1432aea7-f727-4282-9707-23dfbe417d53"), "500", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4412), "302", "Không tồn tại mã RFID", "ETAG_NOTFOUND", "Checkin", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25cf2789-c3f8-48c1-9392-920b3ea5a0a4"), "400", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4414), "301", "Thiếu tham số đầu vào", "", "Checkout", "VETC", "BAD_REQUEST" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333022"), "500", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4415), "305", "Tài khoản không đủ tiền", "NOT_ENOUGH_MONEY", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("2c1ad42f-9c67-4ed3-a2f1-f8b912acc396"), "400", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4417), "301", "Thiếu tham số đầu vào", "", "Commit", "VETC", "BAD_REQUEST" },
                    { new Guid("378a72d0-999e-49e9-bab3-9f68bb591de9"), "500", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4418), "306", "Không tìm thấy giao dịch", "TRANSACTION_NOTFOUND", "Commit", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("7fe27592-d680-41a0-a8a6-0ea9441495a0"), "400", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4419), "301", "Thiếu tham số đầu vào", "", "Rollback", "VETC", "BAD_REQUEST" },
                    { new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b0"), "500", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4421), "308", "Roll back không thành công", "PAYMENT_ERROR", "Rollback", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d64"), "2", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4423), "201", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("e09d4996-85ba-49db-b773-f0ea32590abd"), "27", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4425), "226", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a62f"), "1", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4426), "501", "Khách hàng hủy thanh toán", "", "", "POS", "" },
                    { new Guid("fdf50f73-ede5-4db2-82e1-5e0aa08b6c0e"), "13", new DateTime(2023, 11, 6, 15, 32, 4, 114, DateTimeKind.Local).AddTicks(4427), "513", "Đầy bộ nhớ", "", "", "VDTC", "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorResponse");
        }
    }
}
