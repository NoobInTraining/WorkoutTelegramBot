using System;

namespace WorkoutTelegramBot.Database
{
    public class WorkoutPlan : BaseEntity
    {
        public WorkoutPlan()
        {

        }

        public WorkoutPlan(string type, int inrement, int incrementIntervallInDays)
        {
            Type = type;
            Increment = inrement;
            IncrementIntervallInDays = incrementIntervallInDays;
        }

        public string Type { get; set; }

        public int Increment { get; set; }

        public int IncrementIntervallInDays { get; set; }

        public override bool Equals(object obj)
        {
            return obj is WorkoutPlan plan &&
                   Type == plan.Type &&
                   Increment == plan.Increment &&
                   IncrementIntervallInDays == plan.IncrementIntervallInDays;
        }

        public int GetAmount(DateTime date)
        {
            var multipliyer = date.DayOfYear / IncrementIntervallInDays;
            return multipliyer * Increment;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Increment, IncrementIntervallInDays);
        }

        public string ToString(DateTime date)
        {
            return $"{GetAmount(date),4} {Type}";
        }
    }
}
