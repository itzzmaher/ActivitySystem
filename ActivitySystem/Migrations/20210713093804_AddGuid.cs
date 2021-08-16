using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblUsers_tblColleges_CollegeId",
                table: "tblUsers");

            migrationBuilder.AlterColumn<int>(
                name: "CollegeId",
                table: "tblUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "GuId",
                table: "tblUsers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GuId",
                table: "tblRegisterations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GuId",
                table: "tblActivities",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_tblUsers_tblColleges_CollegeId",
                table: "tblUsers",
                column: "CollegeId",
                principalTable: "tblColleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblUsers_tblColleges_CollegeId",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "GuId",
                table: "tblUsers");

            migrationBuilder.DropColumn(
                name: "GuId",
                table: "tblRegisterations");

            migrationBuilder.DropColumn(
                name: "GuId",
                table: "tblActivities");

            migrationBuilder.AlterColumn<int>(
                name: "CollegeId",
                table: "tblUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_tblUsers_tblColleges_CollegeId",
                table: "tblUsers",
                column: "CollegeId",
                principalTable: "tblColleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
