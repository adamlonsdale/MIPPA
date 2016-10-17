using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class MatchOldResultsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Points",
                table: "TeamResults");

            migrationBuilder.DropColumn(
                name: "Rounds",
                table: "TeamResults");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "PlayerResults");

            migrationBuilder.DropColumn(
                name: "Rounds",
                table: "PlayerResults");

            migrationBuilder.AddColumn<int>(
                name: "Dues",
                table: "TeamResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "TeamResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Wins",
                table: "TeamResults",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "AdjustedScore",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Dues",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dues",
                table: "TeamResults");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "TeamResults");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "TeamResults");

            migrationBuilder.DropColumn(
                name: "AdjustedScore",
                table: "PlayerResults");

            migrationBuilder.DropColumn(
                name: "Dues",
                table: "PlayerResults");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "PlayerResults");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "PlayerResults");

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "TeamResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rounds",
                table: "TeamResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rounds",
                table: "PlayerResults",
                nullable: false,
                defaultValue: 0);
        }
    }
}
