using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WorkoutTelegramBot.Database;

namespace WorkoutTelegramBot
{
    public class TrainingMessage
    {
        static public string GenerateMessage(DailyWorkoutMessage dailyMessage)
        {
            if (dailyMessage.CompletedUsers?.Any() != true)
            {
                return dailyMessage.BaseMessage;
            }
            
            var sb = new StringBuilder(dailyMessage.BaseMessage);
            sb.AppendLine();
            sb.AppendLine("--- Erledigt ---");
            sb.AppendLine();

            var finishedUsersGrouped = dailyMessage.CompletedUsers
                .ToLookup(c => c.WorkoutPlan);

            foreach (var group in finishedUsersGrouped)
            {
                sb.AppendLine($"{group.Key.Type}:");

                foreach (var user in group)
                {
                    sb.AppendLine($" - {user.TelegramUser}");
                }
            }

            return sb.ToString();
        }

        static public InlineKeyboardMarkup GenerateKeyboard(DailyWorkoutMessage dailyMessage, IEnumerable<WorkoutPlan> excercises)
        {
            if (dailyMessage.Id == 0)
            {
                throw new ArgumentException("The Id of the daily message needs to be set before calling.", nameof(dailyMessage));
            }

            var keyboard = excercises
                .Select(e => (e.Type, Excercise: e, Amount: e.GetAmount(dailyMessage.Date) ))
                .OrderBy(e => e.Amount)
                    .ThenBy(e => e.Type)
                .Select(e => e.Excercise)
                .Select(e => new[]
                {
                    InlineKeyboardButton.WithCallbackData(
                        text: e.ToString(dailyMessage.Date),
                        callbackData: new WorkoutCallbackData(e.Id, dailyMessage.Id).ToJson()
                    ),
                })
            ;

            return new InlineKeyboardMarkup(keyboard);
        }
    }
}
