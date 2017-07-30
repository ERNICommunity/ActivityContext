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