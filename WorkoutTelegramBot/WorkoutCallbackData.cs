using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Telegram.Bot.Types;

namespace WorkoutTelegramBot
{
    public class WorkoutCallbackData
    {
        public int WorkoutPlanId { get; set; }
        public int DailyWorkoutMessageId { get; set; }

        [JsonConstructor()]
        private WorkoutCallbackData()
        {
        }


        public WorkoutCallbackData(int workoutPlanId, int dailyWorkoutMessageId)
        {
            WorkoutPlanId = workoutPlanId;
            DailyWorkoutMessageId = dailyWorkoutMessageId;
        }

        public string ToJson()
            => WorkoutCallbackDataFactory.ToJSon(this);
    }
}