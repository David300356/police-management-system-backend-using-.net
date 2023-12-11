﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace police_backend.Migrations
{
    /// <inheritdoc />
    public partial class Eight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest");

            migrationBuilder.DropIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest");

            migrationBuilder.CreateIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest",
                column: "CellListId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest",
                column: "CellListId",
                principalTable: "CellList",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest");

            migrationBuilder.DropIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest");

            migrationBuilder.CreateIndex(
                name: "IX_Arrest_CellListId",
                table: "Arrest",
                column: "CellListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Arrest_CellList_CellListId",
                table: "Arrest",
                column: "CellListId",
                principalTable: "CellList",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
