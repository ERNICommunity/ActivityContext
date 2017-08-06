using System;
using System.Threading.Tasks;
using log4net;
using log4net.Core;

namespace ActivityContext.Integration.log4net
{
    public static class LoggerExtensions
    {
        private static readonly Type ThisDeclaringType = typeof(LoggerExtensions);

        public static void TraceActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Trace, name, action);
        }

        public static void DebugActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Debug, name, action);
        }

        public static void InfoActivity(this ILog logger, string name, Action action)
        {
            LogActivity(logger, Level.Info, name, action);
        }

        public static Task TraceActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Trace, name, action);
        }

        public static Task DebugActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Debug, name, action);
        }

        public static Task InfoActivityAsync(this ILog logger, string name, Func<Task> action)
        {
            return LogActivityAsync(logger, Level.Info, name, action);
        }

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
