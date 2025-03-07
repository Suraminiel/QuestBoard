using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Projects_ProjectsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ProjectsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProjectsId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "AppUserProjects",
                columns: table => new
                {
                    ProjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserProjects", x => new { x.ProjectsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserProjects_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserProjects_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserProjects_UsersId",
                table: "AppUserProjects",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserProjects");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectsId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ProjectsId",
                table: "Users",
                column: "ProjectsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Projects_ProjectsId",
                table: "Users",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
