using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class UndoPrevious_AddSavedToPlayerMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMatchEdited",
                table: "Scorecards");

            migrationBuilder.DropColumn(
                name: "NumberOfTables",
                table: "Scorecards");

            migrationBuilder.AddColumn<bool>(
                name: "Saved",
                table: "PlayerMatches",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saved",
                table: "PlayerMatches");

            migrationBuilder.AddColumn<int>(
                name: "LastMatchEdited",
                table: "Scorecards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfTables",
                table: "Scorecards",
                nullable: false,
                defaultValue: 0);
        }
    }
}
