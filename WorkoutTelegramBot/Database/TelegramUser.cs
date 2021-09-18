using System.ComponentModel.DataAnnotations;

namespace WorkoutTelegramBot.Database
{
    public class TelegramUser : BaseEntity
    {
        [MaxLength(500)]
        public string FirstNameCache { get; set; }
        [MaxLength(500)]
        public string UsernameCache { get; set; }
        [MaxLength(500)]
        public string LastNameCache { get; set; }
        public long TelegramId { get; set; }

        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(FirstNameCache))
            {
                return FirstNameCache;
            }

            if (!string.IsNullOrWhiteSpace(UsernameCache))
            {
                return UsernameCache;
            }

            if (!string.IsNullOrWhiteSpace(LastNameCache))
            {
                return LastNameCache;
            }

            return $"{TelegramId}";
        }
    }

}
