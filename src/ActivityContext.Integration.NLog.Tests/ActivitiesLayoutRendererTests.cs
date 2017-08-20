using System.IO;
using System.Text;
using ActivityContext.Serialization;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;
using Xunit;

namespace ActivityContext.Integration.NLog.Tests
{
    /// <summary>
    /// Simple integration tests whether ActivitiesLayoutRenderer is working as expected.
    /// </summary>
    public class ActivitiesLayoutRendererTests
    {
        public ActivitiesLayoutRendererTests()
        {
            LayoutRenderer.Register<ActivitiesLayoutRenderer>("activities");
        }

        [Fact]
        public void ActivitiesInJsonFormat()
        {
            var target = new MemoryTarget();
            target.Layout = "${activities}";

            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);

            var logger = LogManager.GetCurrentClassLogger();

            using (var activity = new Activity())
            {
                logger.Debug("Test");
                Assert.Equal(1, target.Logs.Count);

                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(target.Logs[0])))
                {
                    var activities = (ActivityInfoList)ActivityInfoList.DefaultJsonSerializer.ReadObject(ms);
                    Assert.Equal(1, activities.Count);
                    Assert.Equal(activity.Id, activities[0].Id);
                    Assert.Equal(activity.Name, activities[0].Name);
                }
            }
        }
    }
}