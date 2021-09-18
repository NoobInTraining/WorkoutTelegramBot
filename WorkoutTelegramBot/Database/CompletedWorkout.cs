namespace WorkoutTelegramBot.Database
{
    public class CompletedWorkout : BaseEntity
    {
        public int TelegramUserId { get; set; }

        public virtual TelegramUser TelegramUser { get; set; }

        public int DailyWorkoutMessageId { get; set; }

        public virtual DailyWorkoutMessage DailyWorkoutMessage { get; set; }

        public int WorkoutPlanId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; } 
    }

}
