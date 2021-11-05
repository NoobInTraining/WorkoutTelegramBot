using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace WorkoutTelegramBot.Database
{
    public class WorkoutContext : DbContext
    {
        #region Public Constructors

        public WorkoutContext([NotNull] DbContextOptions<WorkoutContext> options) : base(options)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public DbSet<GroupChat> Chats { get; set; }

        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; }

        public DbSet<DailyWorkoutMessage> DailyWorkoutMessages { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        public DbSet<TelegramUser> TelegramUsers { get; set; }

        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }

        #endregion Public Properties

        #region Protected Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        #endregion Protected Methods

        #region Private Methods

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutPlan>().HasData(new[]
            {
                new WorkoutPlan("Push-ups", 1, 1) { Id = 1 },
                new WorkoutPlan("Pull-ups", 1, 7) { Id = 2 },
                new WorkoutPlan("Sit-ups", 1, 1) { Id = 3 },
                new WorkoutPlan("Squats", 1, 1) { Id = 4 },
            });

            modelBuilder.Entity<GroupChat>().HasData(new[]
            {
                new GroupChat("Old Group", -1001538744570) { Id = 1 },
                new GroupChat("Chad", -1548157689) { Id = 2 },
                new GroupChat("Legs", -1763737479) { Id = 3 },
                new GroupChat("Chest", -1761385598) { Id = 4 },
                new GroupChat("Arms", -1476505107) { Id = 5 },
                new GroupChat("Abs", -1742607664) { Id = 6 },
            });

            int subscriptionId = 1;
            modelBuilder.Entity<Subscription>().HasData(new[]
            {
                // chest
                new Subscription(4, 1) { Id = subscriptionId++ },
                // arms
                new Subscription(5, 2) { Id = subscriptionId++ },
                // abs
                new Subscription(6, 3) { Id = subscriptionId++ },
                // Legs
                new Subscription(3, 4) { Id = subscriptionId++ },

                // Chad tier
                new Subscription(2, 1) { Id = subscriptionId++ },
                new Subscription(2, 2) { Id = subscriptionId++ },
                new Subscription(2, 3) { Id = subscriptionId++ },
                new Subscription(2, 4) { Id = subscriptionId++ },
            });
        }

        #endregion Private Methods
    }
}