using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutTelegramBot.Migrations
{
    public partial class AddTrackingKapabilites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TelegramMessageId",
                table: "DailyWorkoutMessages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompletedWorkouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TelegramUserId = table.Column<int>(type: "int", nullable: false),
                    DailyWorkoutMessageId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedWorkouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedWorkouts_DailyWorkoutMessages_DailyWorkoutMessageId",
                        column: x => x.DailyWorkoutMessageId,
                        principalTable: "DailyWorkoutMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedWorkouts_TelegramUsers_TelegramUserId",
                        column: x => x.TelegramUserId,
                        principalTable: "TelegramUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedWorkouts_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_DailyWorkoutMessageId",
                table: "CompletedWorkouts",
                column: "DailyWorkoutMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_TelegramUserId",
                table: "CompletedWorkouts",
                column: "TelegramUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedWorkouts_WorkoutPlanId",
                table: "CompletedWorkouts",
                column: "WorkoutPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedWorkouts");

            migrationBuilder.DropColumn(
                name: "TelegramMessageId",
                table: "DailyWorkoutMessages");
        }
    }
}
