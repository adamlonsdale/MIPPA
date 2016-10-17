using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class move_team_login : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "TeamRosters");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TeamRosters");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Teams",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Teams",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "TeamRosters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TeamRosters",
                nullable: true);
        }
    }
}
