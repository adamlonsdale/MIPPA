using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class SessionAddStateRemoveDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Sessions");

            migrationBuilder.AddColumn<bool>(
                name: "ScheduleCreated",
                table: "Sessions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleCreated",
                table: "Sessions");

            migrationBuilder.AddColumn<string>(
                name: "EndDate",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StartDate",
                table: "Sessions",
                nullable: true);
        }
    }
}
