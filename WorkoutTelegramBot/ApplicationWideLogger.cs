using System;
using NLog;
using System.Reflection;

namespace WorkoutTelegramBot
{
    /// <summary>
    /// Provides
    /// </summary>
    public static class ApplicationWideLogger
    {
        #region Public Constructors

        static ApplicationWideLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();
            var executingAssembly = Assembly.GetExecutingAssembly().GetName();
            Version = $"{executingAssembly.Name} - v {executingAssembly.Version}";
            _wrapper = typeof(ApplicationWideLogger);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// The underlining logger
        /// </summary>
        public static Logger Logger { get; }

        /// <summary>
        /// A "dummy" version that u can use while logging - used for LogStart and LogEnd
        /// <para />
        /// Has the format of $"{executingAssembly.Name} - v {executingAssembly.Version}"
        /// </summary>
        public static string Version { get; }

        #endregion Public Properties

        #region Private Fields

        /// <summary>
        /// The type of the <see cref="ApplicationWideLogger" />
        /// </summary>
        /// <remarks>Functionalities using this wrapper are inspired by https://stackoverflow.com/a/7486163/5757162</remarks>
        private static readonly Type _wrapper;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Debug(string)
        /// </summary>
        /// <param name="msg">THe message to write to the log</param>
        static public void Debug(string msg)
        {
            Logger.Log(_wrapper, new LogEventInfo(LogLevel.Debug, Logger.Name, msg));
        }

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Error(string)
        /// </summary>
        /// <param name="any">The exception to log</param>
        static public void Error(Exception any)
        {
            Error(any, string.Empty);
        }

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Error(Exception, string)
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <param name="msg">THe message to write to the log</param>
        static public void Error(Exception ex, string msg)
        {
            var logEvent = new LogEventInfo(LogLevel.Error, Logger.Name, msg);
            logEvent.Exception = ex;
            Logger.Log(_wrapper, logEvent);
        }

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Info
        /// </summary>
        /// <param name="msg">THe message to write to the log</param>
        static public void Info(string msg)
        {
            Logger.Log(_wrapper, new LogEventInfo(LogLevel.Info, Logger.Name, msg));
        }

        /// <summary>
        /// Logs the end of the Application in form of $"{ <see cref="Version" />} finished execution!!"
        /// </summary>
        static public void LogEnd()
        {
            LogEnd(LogLevel.Trace);
        }

        /// <summary>
        /// Logs the start of the Application in form of $"{ <see cref="Version" />} finished execution!"
        /// </summary>
        /// <param name="level">The loglevel to use, defaults to LogLevel.Trace</param>
        static public void LogEnd(LogLevel level)
        {
            Logger.Log(_wrapper, new LogEventInfo(level, Logger.Name, $"{Version} finished execution!"));
        }

        /// <summary>
        /// Logs the end of a method
        /// </summary>
        /// <param name="messageFormat">
        /// A preformatted string, where the name of the calling method is fed into (string.Format)
        /// </param>
        static public void LogMethodEnd(string messageFormat = "Finished method {0}!")
        {
            Trace(string.Format(messageFormat, (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name));
        }

        /// <summary>
        /// Logs the start of a method
        /// </summary>
        /// <param name="messageFormat">
        /// A preformatted string, where the name of the calling method is fed into (string.Format)
        /// </param>
        static public void LogMethodStart(string messageFormat = "Starting method {0}...")
        {
            Trace(string.Format(messageFormat, (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name));
        }

        /// <summary>
        /// Logs the start of the Application in form of $"Starting { <see cref="Version" />}"
        /// </summary>
        static public void LogStart()
        {
            LogStart(LogLevel.Trace);
        }

        /// <summary>
        /// Logs the start of the Application in form of $"Starting { <see cref="Version" />}"
        /// </summary>
        /// <param name="level">The loglevel to use, defaults to LogLevel.Trace</param>
        static public void LogStart(LogLevel level)
        {
            Logger.Log(_wrapper, new LogEventInfo(level, Logger.Name, $"Starting {Version}!"));
        }

        /// <summary>
        /// Wires up to the AppDomain.CurrentDomain.UnhandledException event and logs a Fatal
        /// message when ever there is an error.
        /// </summary>
        /// <param name="message">The message to add to the Log</param>
        public static void LogUnhandledExceptions(string message = "An unhandled terminating exception occurred")
            => LogUnhandledExceptions(LogLevel.Fatal, message);

        /// <summary>
        /// Wires up to the <see cref="AppDomain.CurrentDomain" />.UnhandledException event and logs
        /// when ever there is an error.
        /// </summary>
        /// <param name="logLevel">
        /// The LogLevel to log at, default: <see cref="NLog.LogLevel.Fatal" />
        /// </param>
        /// <param name="message">The message to add to the Log</param>
        public static void LogUnhandledExceptions(LogLevel logLevel, string message = "An unhandled terminating exception occurred")
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var logEvent = new LogEventInfo(logLevel, Logger.Name, message);
                logEvent.Exception = e.ExceptionObject as Exception;
                Logger.Log(_wrapper, logEvent);
            };
        }

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Trace(string)"/&gt;
        /// </summary>
        /// <param name="msg">THe message to write to the log</param>
        static public void Trace(string msg)
        {
            Logger.Log(_wrapper, new LogEventInfo(LogLevel.Trace, Logger.Name, msg));
        }

        /// <summary>
        /// Wrapper for ApplicationWideLogger.Logger.Warn(string)"/&gt;
        /// </summary>
        /// <param name="msg">The message to write to the log</param>
        static public void Warn(string msg)
        {
            Logger.Log(_wrapper, new LogEventInfo(LogLevel.Warn, Logger.Name, msg));
        }

        #endregion Public Methods
    }
}

