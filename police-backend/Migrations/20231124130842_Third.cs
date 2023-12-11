using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace police_backend.Migrations
{
    /// <inheritdoc />
    public partial class Third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finding_Report_ReportId",
                table: "Finding");

            migrationBuilder.AlterColumn<int>(
                name: "ReportId",
                table: "Finding",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Finding_Report_ReportId",
                table: "Finding",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Finding_Report_ReportId",
                table: "Finding");

            migrationBuilder.AlterColumn<int>(
                name: "ReportId",
                table: "Finding",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Finding_Report_ReportId",
                table: "Finding",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "id");
        }
    }
}
