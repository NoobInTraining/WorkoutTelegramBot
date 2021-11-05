using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WorkoutTelegramBot.Database
{
    public class GroupChat : BaseEntity
    {
        #region Public Constructors

        
        public GroupChat()
        {

        }

        public GroupChat(string friendlyName, long telegramGroupChatId)
        {
            FriendlyName = friendlyName;
            TelegramGroupChatId = telegramGroupChatId;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Friendly name for this group chat for a better overview
        /// </summary>
        [MaxLength(1_000)]
        public string FriendlyName { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<DailyWorkoutMessage> DailyWorkoutMessages { get; set; } = new List<DailyWorkoutMessage>();

        public long TelegramGroupChatId { get; set; }

        #endregion Public Properties
    }
}