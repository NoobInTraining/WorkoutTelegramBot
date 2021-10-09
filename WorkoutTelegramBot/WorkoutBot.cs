using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using WorkoutTelegramBot.Database;

namespace WorkoutTelegramBot
{
    public class WorkoutBot : TelegramBotClient
    {
        #region Private Fields

        private DateTime _LastExecution;

        private SemaphoreSlim _LastExecutionLock;

        #endregion Private Fields

        #region Public Constructors

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

        #endregion Public Constructors

        #region Public Properties

        public IDbContextFactory<WorkoutContext> ContextFactory { get; }

        public long GroupChatId { get; }

        public bool IsRunning { get; private set; }

        public WorkoutPressedHandler UpdateHandler { get; }

        #endregion Public Properties

        #region Public Methods

        public void Start()
        {
            if (IsRunning)
            {
                return;
            }

            IsRunning = true;
            this.StartReceiving(UpdateHandler);
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
            ApplicationWideLogger.LogMethodStart();
            using var workoutContext = this.ContextFactory.CreateDbContext();

            ApplicationWideLogger.Debug("Loading daily message;");
            var dailyMessage = await workoutContext.DailyWorkoutMessages.FirstOrDefaultAsync(e => e.Date.Date == DateTime.Now.Date);

            if (dailyMessage != null)
            {
                // message of the day already exists
                ApplicationWideLogger.Debug("Daily message already exists");
                return;
            }

            ApplicationWideLogger.Debug("Creating new daily message..");
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
            ApplicationWideLogger.Debug("Saving..");
            await workoutContext.SaveChangesAsync();

            ApplicationWideLogger.Debug("Sending new telegram message..");
            var message = await this.SendTextMessageAsync(
                chatId: GroupChatId,
                text: TrainingMessage.GenerateMessage(dailyMessage),
                replyMarkup: TrainingMessage.GenerateKeyboard(dailyMessage, excercises)
            );

            ApplicationWideLogger.Debug("Saving message..");
            dailyMessage.TelegramMessageId = message.MessageId;
            await workoutContext.SaveChangesAsync();
            ApplicationWideLogger.LogMethodEnd();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Waits for 1 second before sending the next message
        /// </summary>
        /// <returns></returns>
        private async Task TimeBuffer()
        {
            ApplicationWideLogger.LogMethodStart();
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
            ApplicationWideLogger.LogMethodEnd();
        }

        private async Task UpdateHandler_WorkoutReceived(object sender, WorkoutEventArgs eventArgs)
        {
            ApplicationWideLogger.LogMethodStart();

            await TimeBuffer();

            ApplicationWideLogger.Debug("Creating context..");
            using var workoutContext = ContextFactory.CreateDbContext();

            ApplicationWideLogger.Debug("Fetching DB info..");
            var user = await workoutContext.TelegramUsers.FirstOrDefaultAsync(e => e.TelegramId == eventArgs.User.Id);
            if (user == null)
            {
                ApplicationWideLogger.Info("Creating new User..");
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
                ApplicationWideLogger.Logger.Error("Workout plan nicht gefunden");
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
                ApplicationWideLogger.Logger.Error("Daily Message nicht gefunden");
                System.Diagnostics.Debugger.Break();
            }

            var completedDay = dailyWorkoutMessage.CompletedUsers
                .Where(e => e.TelegramUserId == user.Id)
                .Where(e => e.WorkoutPlanId == workoutPlan.Id)
                .FirstOrDefault();

            if (completedDay == null)
            {
                // finished completing challenge - add him
                ApplicationWideLogger.Info("User completed challenge..");
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
                ApplicationWideLogger.Info("User deleted his completion..");
                workoutContext.Remove(completedDay);
            }

            ApplicationWideLogger.Debug("Saving changes..");
            workoutContext.SaveChanges();

            WorkoutPlan[] excercises = await workoutContext.WorkoutPlans
                    .AsNoTracking()
                    .ToArrayAsync();

            ApplicationWideLogger.Debug("Editing Telegram Message..");
            await this.EditMessageTextAsync(
                chatId: GroupChatId,
                messageId: dailyWorkoutMessage.TelegramMessageId,
                text: TrainingMessage.GenerateMessage(dailyWorkoutMessage),
                replyMarkup: TrainingMessage.GenerateKeyboard(dailyWorkoutMessage, excercises)
            );

            ApplicationWideLogger.Debug("Saving changes..");
            await AnswerCallbackQueryAsync(eventArgs.CallbackQuery.Id);

            ApplicationWideLogger.LogMethodEnd();
        }

        #endregion Private Methods
    }
}