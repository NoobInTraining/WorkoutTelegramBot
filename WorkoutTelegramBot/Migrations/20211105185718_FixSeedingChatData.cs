using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutTelegramBot.Migrations
{
    public partial class FixSeedingChatData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 2,
                column: "TelegramGroupChatId",
                value: -1001548157689L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 3,
                column: "TelegramGroupChatId",
                value: -1001763737479L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 4,
                column: "TelegramGroupChatId",
                value: -1001761385598L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 5,
                column: "TelegramGroupChatId",
                value: -1001476505107L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 6,
                column: "TelegramGroupChatId",
                value: -1001742607664L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 2,
                column: "TelegramGroupChatId",
                value: -1548157689L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 3,
                column: "TelegramGroupChatId",
                value: -1763737479L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 4,
                column: "TelegramGroupChatId",
                value: -1761385598L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 5,
                column: "TelegramGroupChatId",
                value: -1476505107L);

            migrationBuilder.UpdateData(
                table: "Chats",
                keyColumn: "Id",
                keyValue: 6,
                column: "TelegramGroupChatId",
                value: -1742607664L);
        }
    }
}
