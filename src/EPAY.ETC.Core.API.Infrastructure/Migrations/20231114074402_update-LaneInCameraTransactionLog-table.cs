using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class updateLaneInCameraTransactionLogtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CamerraIPAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CameraIPAddr");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CameraIPAddr",
                table: "LaneInCameraTransactionLog",
                newName: "CamerraIPAddr");
        }
    }
}
