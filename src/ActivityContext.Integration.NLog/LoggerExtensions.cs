using System;
using System.Threading.Tasks;
using NLog;

namespace ActivityContext.Integration.NLog
{
    public static class LoggerExtensions
    {
        public static void TraceActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Trace, name, action);
        }

        public static void DebugActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Debug, name, action);
        }

        public static void InfoActivity(this ILogger logger, string name, Action action)
        {
            LogActivity(logger, LogLevel.Info, name, action);
        }

        public static Task TraceActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Trace, name, action);
        }

        public static Task DebugActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Debug, name, action);
        }

        public static Task InfoActivityAsync(this ILogger logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, LogLevel.Info, name, action);
        }

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
