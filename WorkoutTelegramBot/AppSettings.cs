using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WorkoutTelegramBot
{
    public class AppSettings
    {
        [JsonConstructor]
        private AppSettings()
        {

        }

        public AppSettings(string botToken, long groupChatId, string workoutDatabaseConnectionString)
        {
            BotToken = botToken;
            GroupChatId = groupChatId;
            WorkoutDatabaseConnectionString = workoutDatabaseConnectionString;
        }

        public string BotToken { get; set; }

        public long GroupChatId { get; set; }

        public string WorkoutDatabaseConnectionString { get; set; }

        static public async Task<AppSettings> LoadAsync(string path = "appsettings.json")
        {
            var file = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<AppSettings>(file);
        }
    }
}
