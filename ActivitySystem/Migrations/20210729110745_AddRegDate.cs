using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddRegDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterEndDate",
                table: "tblActivities",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterStartDate",
                table: "tblActivities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegisterEndDate",
                table: "tblActivities");

            migrationBuilder.DropColumn(
                name: "RegisterStartDate",
                table: "tblActivities");
        }
    }
}
