using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddSemester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemesterId",
                table: "tblActivities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "tblSemesters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GuId = table.Column<Guid>(nullable: false),
                    SemesterName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSemesters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblActivities_SemesterId",
                table: "tblActivities",
                column: "SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblActivities_tblSemesters_SemesterId",
                table: "tblActivities",
                column: "SemesterId",
                principalTable: "tblSemesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblActivities_tblSemesters_SemesterId",
                table: "tblActivities");

            migrationBuilder.DropTable(
                name: "tblSemesters");

            migrationBuilder.DropIndex(
                name: "IX_tblActivities_SemesterId",
                table: "tblActivities");

            migrationBuilder.DropColumn(
                name: "SemesterId",
                table: "tblActivities");
        }
    }
}
