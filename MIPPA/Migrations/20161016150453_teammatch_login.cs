﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MIPPA_Angular.Migrations
{
    public partial class teammatch_login : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "TeamRosters",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "TeamRosters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "TeamRosters");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "TeamRosters");
        }
    }
}
