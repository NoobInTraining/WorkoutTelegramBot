using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutTelegramBot.Migrations
{
    public partial class AddSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "Increment", "IncrementIntervallInDays", "Type" },
                values: new object[] { 1, 1, 1, "Push-ups" });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "Increment", "IncrementIntervallInDays", "Type" },
                values: new object[] { 2, 1, 7, "Pull-ups" });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "Increment", "IncrementIntervallInDays", "Type" },
                values: new object[] { 3, 1, 1, "Sit-ups" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkoutPlans",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
