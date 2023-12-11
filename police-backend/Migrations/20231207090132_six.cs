using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace police_backend.Migrations
{
    /// <inheritdoc />
    public partial class six : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comment",
                table: "Arrest");

            migrationBuilder.DropColumn(
                name: "hasEvidence",
                table: "Arrest");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Witness",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Suspect",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "Station",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Police",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "CellListId",
                table: "Arrest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Assign",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    PoliceId = table.Column<int>(type: "int", nullable: true),
                    createdOn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assign", x => x.id);
                    table.ForeignKey(
                        name: "FK_Assign_Police_PoliceId",
                        column: x => x.PoliceId,
                        principalTable: "Police",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Assign_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "CellList",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CellNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CellList", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Court",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourtDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocketNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Court", x => x.id);
                    table.ForeignKey(
                        name: "FK_Court_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Outcome",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutcomeName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outcome", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CaseOutcome",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: true),
                    OutcomeId = table.Column<int>(type: "int", nullable: true),
                    isClosed = table.Column<bool>(type: "bit", nullable: false),
                    createdOn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseOutcome", x => x.id);
                    table.ForeignKey(
                        name: "FK_CaseOutcome_Outcome_OutcomeId",
                        column: x => x.OutcomeId,
                        principalTable: "Outcome",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_CaseOutcome_Report_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Report",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Witness_idNumber",
                table: "Witness",
                column: "idNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suspect_idNumber",
                table: "Suspect",
                column: "idNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Station_code",
                table: "Station",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Police_idNumber",
                table: "Police",
                column: "idNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest",
                column: "CellListId");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_PoliceId",
                table: "Assign",
                column: "PoliceId");

            migrationBuilder.CreateIndex(
                name: "IX_Assign_ReportId",
                table: "Assign",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseOutcome_OutcomeId",
                table: "CaseOutcome",
                column: "OutcomeId");

            migrationBuilder.CreateIndex(
                name: "IX_CaseOutcome_ReportId",
                table: "CaseOutcome",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Court_ReportId",
                table: "Court",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest",
                column: "CellListId",
                principalTable: "CellList",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest");

            migrationBuilder.DropTable(
                name: "Assign");

            migrationBuilder.DropTable(
                name: "CaseOutcome");

            migrationBuilder.DropTable(
                name: "CellList");

            migrationBuilder.DropTable(
                name: "Court");

            migrationBuilder.DropTable(
                name: "Outcome");

            migrationBuilder.DropIndex(
                name: "IX_Witness_idNumber",
                table: "Witness");

            migrationBuilder.DropIndex(
                name: "IX_Suspect_idNumber",
                table: "Suspect");

            migrationBuilder.DropIndex(
                name: "IX_Station_code",
                table: "Station");

            migrationBuilder.DropIndex(
                name: "IX_Police_idNumber",
                table: "Police");

            migrationBuilder.DropIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest");

            migrationBuilder.DropColumn(
                name: "CellListId",
                table: "Arrest");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Witness",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Suspect",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "Station",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "idNumber",
                table: "Police",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "comment",
                table: "Arrest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "hasEvidence",
                table: "Arrest",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
