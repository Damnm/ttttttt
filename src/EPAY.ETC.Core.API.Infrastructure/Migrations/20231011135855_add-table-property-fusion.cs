using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPAY.ETC.Core.API.Infrastructure.Migrations
{
    public partial class addtablepropertyfusion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Fusions",
                table: "Fusions");

            migrationBuilder.RenameTable(
                name: "Fusions",
                newName: "Fusion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fusion",
                table: "Fusion",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Fusion",
                table: "Fusion");

            migrationBuilder.RenameTable(
                name: "Fusion",
                newName: "Fusions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fusions",
                table: "Fusions",
                column: "Id");
        }
    }
}
