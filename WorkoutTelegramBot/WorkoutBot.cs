using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using WorkoutTelegramBot.Database;

namespace WorkoutTelegramBot
{
    public class WorkoutBot : TelegramBotClient
    {
        public WorkoutBot(
            string token,
            long groupChatId, 
            IDbContextFactory<WorkoutContext> contextFactory,
            bool start = true,
            HttpClient httpClient = null,
            string baseUrl = null
        ) : base(token, httpClient, baseUrl)
        {
            GroupChatId = groupChatId;
            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            UpdateHandler = new WorkoutPressedHandler();
            UpdateHandler.WorkoutReceivedAsync += UpdateHandler_WorkoutReceived;
            _LastExecutionLock = new SemaphoreSlim(0, 1);
            _LastExecutionLock.Release();
            _LastExecution = DateTime.Now;

            if (start)
            {
                Start();
            }
        }

        private SemaphoreSlim _LastExecutionLock;
        private DateTime _LastExecution;

        public WorkoutPressedHandler UpdateHandler { get; }
        public long GroupChatId { get; }
        public IDbContextFactory<WorkoutContext> ContextFactory { get; }
        public bool IsRunning { get; private set; }

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            this.StartReceiving(UpdateHandler);
        }

        private async Task UpdateHandler_WorkoutReceived(object sender, WorkoutEventArgs eventArgs)
        {
            await TimeBuffer();

            using var workoutContext = ContextFactory.CreateDbContext();

            var user = await workoutContext.TelegramUsers.FirstOrDefaultAsync(e => e.TelegramId == eventArgs.User.Id);
            if (user == null)
            {
                // new user create him
                user = new TelegramUser
                {
                    TelegramId = eventArgs.User.Id,
                    FirstNameCache = eventArgs.User.FirstName,
                    LastNameCache = eventArgs.User.LastName,
                    UsernameCache = eventArgs.User.Username,
                };
            }

            var workoutPlan = await workoutContext.WorkoutPlans
                .FirstOrDefaultAsync(e => e.Id == eventArgs.Workout.WorkoutPlanId);

            if (workoutPlan == null)
            {
                // todo errorhandling
                System.Diagnostics.Debugger.Break();
            }

            var dailyWorkoutMessage = await workoutContext.DailyWorkoutMessages
                .Include(e => e.CompletedUsers)
                    .ThenInclude(e => e.WorkoutPlan)
                .Include(e => e.CompletedUsers)
                    .ThenInclude(e => e.TelegramUser)
                .FirstOrDefaultAsync(e => e.Id == eventArgs.Workout.DailyWorkoutMessageId);

            if (dailyWorkoutMessage == null)
            {
                // todo errorhandling
                System.Diagnostics.Debugger.Break();
            }

            var completedDay = dailyWorkoutMessage.CompletedUsers
                .Where(e => e.TelegramUserId == user.Id)
                .Where(e => e.WorkoutPlanId == workoutPlan.Id)
                .FirstOrDefault();

            if (completedDay == null)
            {
                // finished completing challenge - add him
                dailyWorkoutMessage.CompletedUsers.Add(new CompletedWorkout
                {
                    TelegramUser = user,
                    TelegramUserId = user.Id,
                    WorkoutPlan = workoutPlan,
                    WorkoutPlanId = workoutPlan.Id,
                });
            }
            else
            {
                // already completed, remove him
                workoutContext.Remove(completedDay);
            }

            workoutContext.SaveChanges();

            WorkoutPlan[] excercises = await workoutContext.WorkoutPlans
                    .AsNoTracking()
                    .ToArrayAsync();

            await this.EditMessageTextAsync(
                chatId: GroupChatId,
                messageId: dailyWorkoutMessage.TelegramMessageId,
                text: TrainingMessage.GenerateMessage(dailyWorkoutMessage),
                replyMarkup: TrainingMessage.GenerateKeyboard(dailyWorkoutMessage, excercises)
            );

            await AnswerCallbackQueryAsync(eventArgs.CallbackQuery.Id);
        }
        
        /// <summary>
        /// Waits for 1 second before sending the next message
        /// </summary>
        /// <returns></returns>
        private async Task TimeBuffer()
        {
            await _LastExecutionLock.WaitAsync();
            
            var elapsed = DateTime.Now - _LastExecution;
            var oneSecond = TimeSpan.FromSeconds(1);
            if (elapsed < oneSecond)
            {
                // only send at most once per second
                await Task.Delay(oneSecond - elapsed);
            }

            _LastExecution = DateTime.Now;

            _LastExecutionLock.Release();
        }

        /// <summary>
        /// Checks whether or not a message is sent for the day and sends if it isn't sent
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="groupChatId"></param>
        /// <param name="cts"></param>
        /// <returns></returns>
        public async Task WriteDailyMessage()
        {
            using var workoutContext = this.ContextFactory.CreateDbContext();

            DailyWorkoutMessage dailyMessage = await workoutContext.DailyWorkoutMessages.FirstOrDefaultAsync(e => e.Date.Date == DateTime.Now.Date);

            if (dailyMessage != null)
            {
                // message of the day already exists
                return;
            }

            WorkoutPlan[] excercises = await workoutContext.WorkoutPlans
                    .AsNoTracking()
                    .ToArrayAsync();

            dailyMessage = new DailyWorkoutMessage
            {
                Date = DateTime.Now.Date,
            };
            dailyMessage.SetBaseMessage(excercises);

            // add to tracking and save to get the id
            workoutContext.DailyWorkoutMessages.Add(dailyMessage);
            await workoutContext.SaveChangesAsync();

            var message = await this.SendTextMessageAsync(
                chatId: GroupChatId,
                text: TrainingMessage.GenerateMessage(dailyMessage),
                replyMarkup: TrainingMessage.GenerateKeyboard(dailyMessage, excercises)
            );

            dailyMessage.TelegramMessageId = message.MessageId;
            await workoutContext.SaveChangesAsync();
        }
    }
}