using System;
using ActivityContext.Integration.log4net;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config")]

namespace ActivityContext.Examples.log4net
{
    internal class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        private static void Main()
        {
            GlobalContext.Properties["activities"] = new ActivitiesProperty();

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
                    Logger.Warn("FailTest throw an exception (as expected)", ex);
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
