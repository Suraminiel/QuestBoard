using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddedDomainModelsforForumThreadsandPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "forumThreads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forumThreads", x => x.id);
                    table.ForeignKey(
                        name: "FK_forumThreads_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumPosts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThreadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumPosts", x => x.id);
                    table.ForeignKey(
                        name: "FK_ForumPosts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumPosts_forumThreads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "forumThreads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_ThreadId",
                table: "ForumPosts",
                column: "ThreadId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumPosts_UserId",
                table: "ForumPosts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_forumThreads_ProjectId",
                table: "forumThreads",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumPosts");

            migrationBuilder.DropTable(
                name: "forumThreads");
        }
    }
}
