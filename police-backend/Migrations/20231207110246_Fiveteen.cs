using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace police_backend.Migrations
{
    /// <inheritdoc />
    public partial class Fiveteen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenderId",
                table: "Report",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    gName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Report_GenderId",
                table: "Report",
                column: "GenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Gender_GenderId",
                table: "Report",
                column: "GenderId",
                principalTable: "Gender",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Report_Gender_GenderId",
                table: "Report");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropIndex(
                name: "IX_Report_GenderId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "GenderId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "caseNumber",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "nationality",
                table: "Report");
        }
    }
}
