using System;
using System.Threading.Tasks;
using NLog;

namespace ActivityContext.Integration.NLog
{
    /// <summary>
    /// Provides extension methods to simplify work with <see cref="Activity"/> and <see cref="ILogger"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Trace"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void TraceActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Trace, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Debug"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void DebugActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Debug, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Info"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void InfoActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Info, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Trace"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task TraceActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Trace, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Debug"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task DebugActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Debug, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="LogLevel.Info"/> level.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task InfoActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Info, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged.
        /// Any exception will be logged using <see cref="LogLevel.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="level">Level used for execution begin/end logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void LogActivity(this ILogger logger, LogLevel level, string name, Action action)
        {
            using (var activity = new Activity(name))
            {
                var isEnabled = logger.IsEnabled(level);

                if (isEnabled)
                {
                    logger.Log(level, BeginMsg(name));
                }

                try
                {
                    action();

                    if (isEnabled)
                    {
                        logger.Log(level, CompleteMsg(name));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, FailedMsg(name), name);
                    throw new ActivityFailedException(activity, ex);
                }
            }
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged.
        /// Any exception will be logged and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="level">Level used for execution begin/end logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static async Task LogActivityAsync(this ILogger logger, LogLevel level, string name, Func<Task> action)
        {
            using (var activity = new Activity(name))
            {
                var isEnabled = logger.IsEnabled(level);

                if (isEnabled)
                {
                    logger.Log(level, BeginMsg(name));
                }

                try
                {
                    await action();

                    if (isEnabled)
                    {
                        logger.Log(level, CompleteMsg(name));
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, FailedMsg(name), name);
                    throw new ActivityFailedException(activity, ex);
                }
            }
        }

        // In this simple cases, string concatenation should perform better than format strings.
        private static string BeginMsg(string name) => "BEGIN: " + name + ".";

        private static string CompleteMsg(string name) => "COMPLETE: " + name + ".";

        private static string FailedMsg(string name) => "FAILED: " + name + ".";

    }
}
