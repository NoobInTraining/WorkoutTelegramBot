namespace WorkoutTelegramBot.Database
{
    /// <summary>
    /// A <see cref="Chat"/> can subscribe to multiple <see cref="Workout"/>
    /// </summary>
    public class Subscription : BaseEntity
    {
        #region Public Constructors

        public Subscription()
        {
        }

        public Subscription(int chatId, int workoutPlanId)
        {
            ChatId = chatId;
            WorkoutPlanId = workoutPlanId;
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual GroupChat Chat { get; set; }

        public int ChatId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public int WorkoutPlanId { get; set; }

        #endregion Public Properties
    }
}