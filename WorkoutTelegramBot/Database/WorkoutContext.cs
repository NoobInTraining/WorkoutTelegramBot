using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WorkoutTelegramBot.Database
{
    public class WorkoutContext : DbContext
    {
        public WorkoutContext([NotNull] DbContextOptions<WorkoutContext> options) : base(options)
        {
        }

        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<DailyWorkoutMessage> DailyWorkoutMessages { get; set; }
        public DbSet<TelegramUser> TelegramUsers { get; set; }
        public DbSet<CompletedWorkout> CompletedWorkouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutPlan>().HasData(new[]
            {
                new WorkoutPlan("Push-ups", 1, 1) { Id = 1 },
                new WorkoutPlan("Pull-ups", 1, 7) { Id = 2 },
                new WorkoutPlan("Sit-ups", 1, 1) { Id = 3 },
            });
        }
    }

}