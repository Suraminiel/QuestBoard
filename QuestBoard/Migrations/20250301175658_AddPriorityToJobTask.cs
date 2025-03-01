using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestBoard.Migrations
{
    /// <inheritdoc />
    public partial class AddPriorityToJobTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "JobsAndTasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "JobsAndTasks");
        }
    }
}
