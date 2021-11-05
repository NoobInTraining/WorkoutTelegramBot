using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutTelegramBot.Migrations
{
    public partial class AddChatAndSubscriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TelegramGroupChatId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "FriendlyName", "TelegramGroupChatId" },
                values: new object[,]
                {
                    { 1, "Old Group", -1001538744570L },
                    { 2, "Chad", -1548157689L },
                    { 3, "Legs", -1763737479L },
                    { 4, "Chest", -1761385598L },
                    { 5, "Arms", -1476505107L },
                    { 6, "Abs", -1742607664L }
                });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "Increment", "IncrementIntervallInDays", "Type" },
                values: new object[] { 4, 1, 1, "Squats" });

            migrationBuilder.InsertData(
                table: "Subscriptions",
                columns: new[] { "Id", "ChatId", "WorkoutPlanId" },
                values: new object[,]
                {
                    { 5, 2, 1 },
                    { 6, 2, 2 },
                    { 7, 2, 3 },
                    { 1, 4, 1 },
                    { 2, 5, 2 },
                    { 3, 6, 3 },
                    { 4, 3, 4 },
                    { 8, 2, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_ChatId",
                table: "Subscriptions",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_WorkoutPlanId",
                table: "Subscriptions",
                column: "WorkoutPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
