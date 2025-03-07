using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class onetomanybetweenProjectsandTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "JobsAndTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobsAndTasks_ProjectId",
                table: "JobsAndTasks",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobsAndTasks_Projects_ProjectId",
                table: "JobsAndTasks",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobsAndTasks_Projects_ProjectId",
                table: "JobsAndTasks");

            migrationBuilder.DropIndex(
                name: "IX_JobsAndTasks_ProjectId",
                table: "JobsAndTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "JobsAndTasks");
        }
    }
}
