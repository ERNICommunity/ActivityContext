using System;
using ActivityContext.Integration.NLog;
using NLog;

namespace ActivityContext.Examples.NLog
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private static void Main()
        {
            Logger.DebugActivity("Main", () =>
            {
                using (new Activity("Hello"))
                {
                    Logger.Info("Hello World!");
                }

                try
                {
                    Logger.InfoActivity("FailTest", Fail);
                }
                catch (ActivityFailedException ex)
                {
                    Logger.Warn(ex, "FailTest throw an exception (as expected)");
                }
            });
        }

        private static void Fail()
        {
            // Some code...
            throw new Exception("Something gone wrong...");
        }
    }
}
