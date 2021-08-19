﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddIsGraduate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGraduate",
                table: "tblUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGraduate",
                table: "tblUsers");
        }
    }
}
