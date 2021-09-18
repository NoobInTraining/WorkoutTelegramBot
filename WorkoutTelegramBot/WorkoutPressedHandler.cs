using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace WorkoutTelegramBot
{
    public class WorkoutPressedHandler : IUpdateHandler
    {
        public UpdateType[] AllowedUpdates =>  new[] { UpdateType.CallbackQuery };

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Exception caught {exception}");
            return Task.CompletedTask;
        }

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            switch (update.Type)
            {
                case UpdateType.CallbackQuery:
                    await CallbackQueryReceived(update.CallbackQuery);
                    return;

                default:
                    await Log("Unsupported Update received!");
                    return;
            }
        }

        private Task Log(string v)
        {
            Console.WriteLine(v);
            return Task.CompletedTask;
        }

        private async Task CallbackQueryReceived(CallbackQuery callbackQuery)
        {
            var user = callbackQuery.From;
            var workout = WorkoutCallbackDataFactory.FromJson(callbackQuery.Data);
            //callbackQuery.

            await Log($"Received {workout.WorkoutPlanId} from {user.Username}");

            // Call syncronous events
            var args = new WorkoutEventArgs(user, workout, callbackQuery);
            WorkoutReceived?.Invoke(this, args);

            // call asnyc events - inspired by https://stackoverflow.com/a/27763068/5757162
            if (WorkoutReceivedAsync == null)
            {
                return;
            }

            Delegate[] invocationList = WorkoutReceivedAsync.GetInvocationList();
            Task[] handlerTasks = new Task[invocationList.Length];

            for (int i = 0; i < invocationList.Length; i++)
            {
                handlerTasks[i] = ((Func<object, WorkoutEventArgs, Task>)invocationList[i])(this, args);
            }

            await Task.WhenAll(handlerTasks);
        }

        public event EventHandler<WorkoutEventArgs> WorkoutReceived;

        public event Func<object, WorkoutEventArgs, Task> WorkoutReceivedAsync;
    }
}