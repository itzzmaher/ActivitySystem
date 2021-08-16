using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddIsdone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "tblSemesters",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "tblSemesters");
        }
    }
}
