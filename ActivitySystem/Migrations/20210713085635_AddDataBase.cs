using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ActivitySystem.Migrations
{
    public partial class AddDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblColleges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollegeName = table.Column<string>(nullable: true),
                    CollegeCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblColleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    KfuEmail = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    CollegeId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblUsers_tblColleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "tblColleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblUsers_tblRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "tblRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tblActivities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityName = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    MaxStudents = table.Column<int>(nullable: false),
                    CreatorId = table.Column<int>(nullable: false),
                    SupervisorId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblActivities_tblUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblActivities_tblUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tblActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblActivityLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblActivityLogs_tblActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "tblActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblActivityLogs_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tblRegisterations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterationTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    StatusRegId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRegisterations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRegisterations_tblActivities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "tblActivities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblRegisterations_tblStatus_StatusRegId",
                        column: x => x.StatusRegId,
                        principalTable: "tblStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblRegisterations_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "tblRegisterationsLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    RegisterationId = table.Column<int>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRegisterationsLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblRegisterationsLogs_tblRegisterations_RegisterationId",
                        column: x => x.RegisterationId,
                        principalTable: "tblRegisterations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblRegisterationsLogs_tblStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "tblStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_tblRegisterationsLogs_tblUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "tblUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblActivities_CreatorId",
                table: "tblActivities",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblActivities_SupervisorId",
                table: "tblActivities",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblActivityLogs_ActivityId",
                table: "tblActivityLogs",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblActivityLogs_UserId",
                table: "tblActivityLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterations_ActivityId",
                table: "tblRegisterations",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterations_StatusRegId",
                table: "tblRegisterations",
                column: "StatusRegId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterations_UserId",
                table: "tblRegisterations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterationsLogs_RegisterationId",
                table: "tblRegisterationsLogs",
                column: "RegisterationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterationsLogs_StatusId",
                table: "tblRegisterationsLogs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRegisterationsLogs_UserId",
                table: "tblRegisterationsLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUsers_CollegeId",
                table: "tblUsers",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUsers_RoleId",
                table: "tblUsers",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblActivityLogs");

            migrationBuilder.DropTable(
                name: "tblRegisterationsLogs");

            migrationBuilder.DropTable(
                name: "tblRegisterations");

            migrationBuilder.DropTable(
                name: "tblActivities");

            migrationBuilder.DropTable(
                name: "tblStatus");

            migrationBuilder.DropTable(
                name: "tblUsers");

            migrationBuilder.DropTable(
                name: "tblColleges");

            migrationBuilder.DropTable(
                name: "tblRoles");
        }
    }
}
