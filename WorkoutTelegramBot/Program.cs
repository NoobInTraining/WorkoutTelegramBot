using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using System.Linq;
using WorkoutTelegramBot.Database;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WorkoutTelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Reading appsettings..");
            var settings = await AppSettings.LoadAsync();
            Console.WriteLine("Initializing botclient...");
            var contextFactory = new WorkoutContextFactory(settings.WorkoutDatabaseConnectionString);
            using (var context = contextFactory.CreateDbContext())
            {
                Console.WriteLine("Applying migrations..");
                await context.Database.MigrateAsync();
            }

            var botClient = new WorkoutBot(settings.BotToken, settings.GroupChatId, contextFactory);

            do
            {
                Console.WriteLine("Writing daily message..");
                await botClient.WriteDailyMessage();

                var tomorow = DateTime.Now.AddDays(1);
                var fiveAmTomorrow = new DateTime(tomorow.Year, tomorow.Month, tomorow.Day, 0 , 5, 0);
                var timeTowait = fiveAmTomorrow - DateTime.Now;

                Console.WriteLine($"Waiting ~{timeTowait:hh\\:mm} before writing again");
                await Task.Delay(timeTowait);
            } while (true);
        }
    }
}

