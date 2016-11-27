using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class AddByeToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Bye",
                table: "Teams",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bye",
                table: "Teams");
        }
    }
}
