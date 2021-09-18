using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WorkoutTelegramBot.Database
{
    /// <summary>
    /// A daily workout message is 
    /// </summary>
    /// <remarks>
    /// A message is split into two hafes:
    /// The upper and the lower message as shwon below <para/>
    /// 
    /// 20.20.2021 - Heute steht an:<br/>
    ///  - "".PadLeft(3) type<br/>
    /// <br/>
    /// --- Erledigt ---<br/>
    /// <br/>
    /// Pushups:<br/>
    ///  - user 1 <br/>
    ///  - user 2<br/>
    /// <br/>
    /// Pullups:<br/>
    ///  - User 1<br/>
    ///  - User 4<br/>
    /// </remarks>
    public class DailyWorkoutMessage : BaseEntity
    {
        public DateTime Date { get; set; }

        [MaxLength(2000)]
        public string BaseMessage { get; set; }

        public ICollection<CompletedWorkout> CompletedUsers { get; set; }

        public int TelegramMessageId { get; set; }

        /// <summary>
        /// Sets the <see cref="BaseMessage"/> and returns it
        /// </summary>
        /// <param name="excercises"></param>
        /// <returns></returns>
        internal string SetBaseMessage(IEnumerable<WorkoutPlan> excercises = null)
        {
            BaseMessage = $"{Date:dd.MM.yyyy}{Environment.NewLine}Get working you scallywags";
            return BaseMessage;
        }
    }

}
