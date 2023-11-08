using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addnewdataErrorResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333022"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("e09d4996-85ba-49db-b773-f0ea32590abd"));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ErrorResponse",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Function",
                table: "ErrorResponse",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorCode",
                table: "ErrorResponse",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpayMessage",
                table: "ErrorResponse",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpayCode",
                table: "ErrorResponse",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ErrorResponse",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ErrorResponse",
                columns: new[] { "Id", "Code", "CreatedDate", "EpayCode", "EpayMessage", "ErrorCode", "Function", "Source", "Status" },
                values: new object[,]
                {
                    { new Guid("1432aea7-f727-4282-9707-23dfbe417d54"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "303", "Không tìm thấy phương tiện trong bảng giá", "VEHICAL_NOTFOUND", "Checkin", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333023"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "302", "Không tồn tại mã RFID", "ETAG_NOTFOUND", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333024"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "302", "Không tồn tại mã RFID", "ETAG_NOTFOUND", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333025"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "303", "Không tìm thấy phương tiện trong bảng giá", "VEHICAL_NOTFOUND", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333026"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "304", "Thanh toán thành công", "PAYMENT_ERROR", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333027"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "305", "Tài khoản không đủ tiền", "NOT_ENOUGH_MONEY", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("378a72d0-999e-49e9-bab3-9f68bb591de8"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "307", "Commit giao dịch không thành công", "PAYMENT_ERROR", "Commit", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b1"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "306", "Không tìm thấy giao dịch", "TRANSACTION_NOTFOUND", "Rollback", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c10"), "37", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "236", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c11"), "38", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "237", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c12"), "0", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "238", "Tài khoản không đủ tiền", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d65"), "3", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "202", "Vé đã tồn tại", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d66"), "4", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "203", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d67"), "5", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "204", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d68"), "6", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "205", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d69"), "6", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "205", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d70"), "7", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "206", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d71"), "7", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "206", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d72"), "8", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "207", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d73"), "10", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "209", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d74"), "11", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "210", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d75"), "12", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "211", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d76"), "14", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "213", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d77"), "15", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "214", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d78"), "16", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "215", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d79"), "17", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "216", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d80"), "18", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "217", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d81"), "19", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "218", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d82"), "20", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "219", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d83"), "21", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "220", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d84"), "22", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "221", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d85"), "23", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "222", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d86"), "24", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "223", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d88"), "25", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "224", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d89"), "26", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "225", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d90"), "27", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "226", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d91"), "28", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "227", "Không cho phép sử dụng dịch vụ", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d92"), "29", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "228", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d93"), "30", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "229", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d94"), "31", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "230", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d95"), "32", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "231", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d96"), "33", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "232", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d97"), "34", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "233", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d98"), "35", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "234", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d99"), "36", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "235", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d46d72"), "9", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "208", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("a0a7891e-2073-4a9d-b1be-5fcf80d45d76"), "13", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "212", "Lỗi hệ thống", "", "", "VDTC", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a63f"), "3", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "503", "Lỗi hệ thống", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a64f"), "4", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "504", "Lỗi hệ thống", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a65f"), "5", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "505", "Tạo giao dịch bị lỗi", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a66f"), "6", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "506", "Lỗi đọc thẻ", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a67f"), "7", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "507", "Yêu cầu không hợp lệ", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a68f"), "8", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "508", "Lỗi hệ thống", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a69f"), "9", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "509", "Quá nhiều thẻ", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a70f"), "11", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "511", "Số trace không hợp lệ", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a71f"), "12", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "512", "Không được phép hủy", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a73f"), "14", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "514", "Sai định dạng", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a74f"), "15", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "515", "Thẻ hết hạn", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a75f"), "16", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "516", "Lỗi đọc file", "", "", "POS", "" },
                    { new Guid("f8de22ef-2f65-43fc-afd5-defb86f9a63f"), "2", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "502", "Giao dịch thất bại", "", "", "POS", "" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("1432aea7-f727-4282-9707-23dfbe417d54"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333023"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333024"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333025"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333026"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("25ee8f5e-c899-4b55-a894-805dc3333027"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("378a72d0-999e-49e9-bab3-9f68bb591de8"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("9ee064f5-3053-4367-822f-ecf7e2d230b1"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c10"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c11"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45c12"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d65"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d66"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d67"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d68"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d69"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d70"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d71"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d72"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d73"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d74"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d75"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d76"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d77"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d78"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d79"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d80"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d81"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d82"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d83"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d84"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d85"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d86"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d88"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d89"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d90"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d91"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d92"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d93"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d94"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d95"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d96"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d97"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d98"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d45d99"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf79d46d72"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("a0a7891e-2073-4a9d-b1be-5fcf80d45d76"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a63f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a64f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a65f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a66f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a67f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a68f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a69f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a70f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a71f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a73f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a74f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb85f9a75f"));

            migrationBuilder.DeleteData(
                table: "ErrorResponse",
                keyColumn: "Id",
                keyValue: new Guid("f8de22ef-2f65-43fc-afd5-defb86f9a63f"));

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Function",
                table: "ErrorResponse",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ErrorCode",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpayMessage",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EpayCode",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ErrorResponse",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "ErrorResponse",
                columns: new[] { "Id", "Code", "CreatedDate", "EpayCode", "EpayMessage", "ErrorCode", "Function", "Source", "Status" },
                values: new object[,]
                {
                    { new Guid("25ee8f5e-c899-4b55-a894-805dc3333022"), "500", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "305", "Tài khoản không đủ tiền", "NOT_ENOUGH_MONEY", "Checkout", "VETC", "INTERNAL_SERVER_ERROR" },
                    { new Guid("e09d4996-85ba-49db-b773-f0ea32590abd"), "27", new DateTime(2023, 12, 31, 23, 59, 59, 0, DateTimeKind.Utc), "226", "Lỗi hệ thống", "", "", "VDTC", "" }
                });
        }
    }
}
