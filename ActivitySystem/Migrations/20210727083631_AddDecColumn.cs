using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddDecColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "tblActivities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "tblActivities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "tblActivities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "tblActivities");
        }
    }
}
