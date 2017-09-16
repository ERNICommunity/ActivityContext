using System;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace ActivityContext.Integration.log4net
{
    /// <summary>
    /// Provides extension methods to simplify work with <see cref="Activity"/> and <see cref="ILog"/>.
    /// </summary>
    public static class LoggerExtensions
    {
        private static readonly Type ThisDeclaringType = typeof(LoggerExtensions);

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Trace"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void TraceActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Trace, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Debug"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void DebugActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Debug, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Info"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void InfoActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Info, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Trace"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task TraceActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Trace, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Debug"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task DebugActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Debug, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged using <see cref="Level.Info"/> level.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static Task InfoActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Info, name, action);
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="level">Level used for execution begin/end logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static void LogActivity(this ILog logger, Level level, string name, Action action)
        {
            using (var activity = new Activity(name))
            {
                var isEnabled = logger.Logger.IsEnabledFor(level);

                if (isEnabled)
                {
                    logger.Logger.Log(ThisDeclaringType, level, BeginMsg(name), null);
                }

                try
                {
                    action();
                    if (isEnabled)
                    {
                        logger.Logger.Log(ThisDeclaringType, level, CompleteMsg(name), null);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(FailedMsg(name), ex);
                    throw new ActivityFailedException(activity, ex);
                }
            }
        }

        /// <summary>
        /// Executes provided <paramref name="action"/> inside a new <see cref="Activity"/>.
        /// Begin and end of the execution is logged.
        /// Any exception will be logged using <see cref="Level.Error"/> level and then wrapped and re-thrown as <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="logger">Logger used to write logs.</param>
        /// <param name="level">Level used for execution begin/end logs.</param>
        /// <param name="name">Name of the activity.</param>
        /// <param name="action">Action to execute.</param>
        public static async Task LogActivityAsync(this ILog logger, Level level, string name, Func<Task> action)
        {
            using (var activity = new Activity(name))
            {
                var isEnabled = logger.Logger.IsEnabledFor(level);

                if (isEnabled)
                {
                    logger.Logger.Log(ThisDeclaringType, level, BeginMsg(name), null);
                }

                try
                {
                    await action();
                    if (isEnabled)
                    {
                        logger.Logger.Log(ThisDeclaringType, level, CompleteMsg(name), null);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(FailedMsg(name), ex);
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
