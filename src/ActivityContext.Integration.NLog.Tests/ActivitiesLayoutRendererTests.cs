using ActivityContext.Serialization;
using NLog;
using NLog.Config;
using NLog.LayoutRenderers;
using NLog.Targets;
using System.IO;
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

                using (var output = new StringWriter())
                {
                    ActivityJsonSerializer.Write(new[] { activity }, output);
                    var expected = output.ToString();
                    Assert.Equal(expected, target.Logs[0]);
                }
            }
        }
    }
}