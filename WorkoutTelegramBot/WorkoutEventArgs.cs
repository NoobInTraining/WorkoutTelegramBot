using System;
using Telegram.Bot.Types;

namespace WorkoutTelegramBot
{
    public class WorkoutEventArgs : EventArgs
    {

        public WorkoutEventArgs(User user, WorkoutCallbackData workout, CallbackQuery callbackQuery)
        {
            User = user;
            Workout = workout;
            this.CallbackQuery = callbackQuery;
        }

        public User User { get; }
        public WorkoutCallbackData Workout { get; }
        public CallbackQuery CallbackQuery { get; }
    }

}