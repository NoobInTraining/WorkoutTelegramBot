using System.Linq;
using System.Text.Json;
using Telegram.Bot.Types.ReplyMarkups;

namespace WorkoutTelegramBot
{
    public class WorkoutCallbackDataFactory
    {
        static public string ToJSon(WorkoutCallbackData workoutResult)
        {
            return JsonSerializer.Serialize(workoutResult);

        }

        static public WorkoutCallbackData FromJson(string workoutResultJson)
        {
            return JsonSerializer.Deserialize<WorkoutCallbackData>(workoutResultJson);
        }
    }
}
