using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace police_backend.Migrations
{
    /// <inheritdoc />
    public partial class Sixteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "caseNumber",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "nationality",
                table: "Report");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "caseNumber",
                table: "Report",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "nationality",
                table: "Report",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
