using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoJWT.SqliteDatabase.Migrations
{
    /// <inheritdoc />
    public partial class TasksUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoTask_Users_UserID",
                table: "TodoTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask");

            migrationBuilder.RenameTable(
                name: "TodoTask",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_TodoTask_UserID",
                table: "Tasks",
                newName: "IX_Tasks_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "TodoTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserID",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "TodoTask");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_UserID",
                table: "TodoTask",
                newName: "IX_TodoTask_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoTask",
                table: "TodoTask",
                column: "TodoTaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoTask_Users_UserID",
                table: "TodoTask",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
