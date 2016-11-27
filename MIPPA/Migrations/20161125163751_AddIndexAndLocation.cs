using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class AddIndexAndLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "TeamMatches",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ResetRequests_ScorecardId",
                table: "ResetRequests",
                column: "ScorecardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ResetRequests_Scorecards_ScorecardId",
                table: "ResetRequests",
                column: "ScorecardId",
                principalTable: "Scorecards",
                principalColumn: "ScorecardId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ResetRequests_Scorecards_ScorecardId",
                table: "ResetRequests");

            migrationBuilder.DropIndex(
                name: "IX_ResetRequests_ScorecardId",
                table: "ResetRequests");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "TeamMatches");
        }
    }
}
