using System.IO;
using System.Text;
using ActivityContext.Data;
using NLog;
using NLog.LayoutRenderers;

namespace ActivityContext.Integration.NLog
{
    /// <summary>
    /// A Layout renderer that outputs JSON serialized list of current activities.    
    /// <seealso href="https://github.com/nlog/nlog/wiki/Layout-Renderers"/>
    /// <seealso cref="Activity.GetCurrentActivities"/>
    /// </summary>
    [LayoutRenderer("activities")]
    public class ActivitiesLayoutRenderer : LayoutRenderer
    {
        /// <summary>
        /// Renders the current activities and appends it to the specified <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder"><see cref="StringBuilder"/> to append the rendered data to</param>
        /// <param name="logEvent">Logging event.</param>
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