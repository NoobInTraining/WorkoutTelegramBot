using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutTelegramBot.Migrations
{
    public partial class DailyWorkoutMessage_AddGroupChatReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupChatId",
                table: "DailyWorkoutMessages",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_DailyWorkoutMessages_GroupChatId",
                table: "DailyWorkoutMessages",
                column: "GroupChatId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyWorkoutMessages_Chats_GroupChatId",
                table: "DailyWorkoutMessages",
                column: "GroupChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyWorkoutMessages_Chats_GroupChatId",
                table: "DailyWorkoutMessages");

            migrationBuilder.DropIndex(
                name: "IX_DailyWorkoutMessages_GroupChatId",
                table: "DailyWorkoutMessages");

            migrationBuilder.DropColumn(
                name: "GroupChatId",
                table: "DailyWorkoutMessages");
        }
    }
}
