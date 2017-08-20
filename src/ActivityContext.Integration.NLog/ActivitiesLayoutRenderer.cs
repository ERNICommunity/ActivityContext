using System.IO;
using System.Text;
using ActivityContext.Serialization;
using NLog;
using NLog.LayoutRenderers;

namespace ActivityContext.Integration.NLog
{
    [LayoutRenderer("activities")]
    public class ActivitiesLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var activities = Activity.GetCurrentActivities();
            var serializer = ActivityInfoList.DefaultJsonSerializer;

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, activities);
                var json = Encoding.UTF8.GetString(ms.ToArray());
                builder.Append(json);
            }
        }
    }
}