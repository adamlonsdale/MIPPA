using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MIPPA_Angular.Migrations
{
    public partial class AddingTeamPlayer_Results : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayerResults",
                columns: table => new
                {
                    PlayerResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Format = table.Column<int>(nullable: false),
                    Handicap = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false),
                    Points = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: false),
                    ScorecardId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerResults", x => x.PlayerResultId);
                    table.ForeignKey(
                        name: "FK_PlayerResults_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerResults_Scorecards_ScorecardId",
                        column: x => x.ScorecardId,
                        principalTable: "Scorecards",
                        principalColumn: "ScorecardId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamResults",
                columns: table => new
                {
                    TeamResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Points = table.Column<int>(nullable: false),
                    Rounds = table.Column<int>(nullable: false),
                    ScorecardId = table.Column<int>(nullable: false),
                    TeamId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamResults", x => x.TeamResultId);
                    table.ForeignKey(
                        name: "FK_TeamResults_Scorecards_ScorecardId",
                        column: x => x.ScorecardId,
                        principalTable: "Scorecards",
                        principalColumn: "ScorecardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamResults_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResults_PlayerId",
                table: "PlayerResults",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerResults_ScorecardId",
                table: "PlayerResults",
                column: "ScorecardId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResults_ScorecardId",
                table: "TeamResults",
                column: "ScorecardId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamResults_TeamId",
                table: "TeamResults",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerResults");

            migrationBuilder.DropTable(
                name: "TeamResults");
        }
    }
}
