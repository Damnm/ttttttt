using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class dbupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentReferenceId",
                table: "PaymentStatus",
                newName: "TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentStatus_PaymentReferenceId",
                table: "PaymentStatus",
                newName: "IX_PaymentStatus_TransactionId");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "ManualBarrierControl",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "PaymentStatus",
                newName: "PaymentReferenceId");

            migrationBuilder.RenameIndex(
                name: "IX_PaymentStatus_TransactionId",
                table: "PaymentStatus",
                newName: "IX_PaymentStatus_PaymentReferenceId");

            migrationBuilder.AlterColumn<Guid>(
                name: "EmployeeId",
                table: "ManualBarrierControl",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
