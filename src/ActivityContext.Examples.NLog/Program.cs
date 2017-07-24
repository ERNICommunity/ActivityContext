using NLog;

namespace ActivityContext.Examples.NLog
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            using (new Activity("Main"))
            {
                Logger.Debug("Program started.");

                using (new Activity("Hello"))
                {
                    Logger.Info("Hello World!");
                }

                Logger.Debug("Program finished.");
            }
        }
    }
}