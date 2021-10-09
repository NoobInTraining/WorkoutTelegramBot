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
            try
            {
                ApplicationWideLogger.LogUnhandledExceptions();
                ApplicationWideLogger.Trace("Reading appsettings..");
                var settings = await AppSettings.LoadAsync();
                ApplicationWideLogger.Trace("Initializing botclient...");
                var contextFactory = new WorkoutContextFactory(settings.WorkoutDatabaseConnectionString);
                using (var context = contextFactory.CreateDbContext())
                {
                    ApplicationWideLogger.Trace("Applying migrations..");
                    await context.Database.MigrateAsync();
                }

                var botClient = new WorkoutBot(settings.BotToken, settings.GroupChatId, contextFactory);

                do
                {
                    ApplicationWideLogger.Trace("Writing daily message..");
                    await botClient.WriteDailyMessage();

                    var tomorow = DateTime.Now.AddDays(1);
                    var fiveAmTomorrow = new DateTime(tomorow.Year, tomorow.Month, tomorow.Day, 0, 5, 0);
                    var timeTowait = fiveAmTomorrow - DateTime.Now;

                    ApplicationWideLogger.Trace($"Waiting ~{timeTowait:hh\\:mm} before writing again");
                    await Task.Delay(timeTowait);
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Unhandled terminating exception!");
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }
    }
}

