using ActivityContext.Serialization;
using NLog;
using NLog.LayoutRenderers;
using System.IO;
using System.Text;

namespace ActivityContext.Integration.NLog
{
    [LayoutRenderer("activities")]
    public class ActivitiesLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            using (var output = new StringWriter(builder))
            {
                var activities = Activity.GetCurrentActivities();
                ActivityJsonSerializer.Write(activities, output);
            }
        }
    }
}