using System.IO;
using System.Text;
using ActivityContext.Data;

namespace ActivityContext.Integration.log4net
{
    /// <summary>
    /// An active property that outputs JSON serialized list of current activities.
    /// <seealso href="https://logging.apache.org/log4net/release/manual/contexts.html"/>
    /// <seealso cref="Activity.GetCurrentActivities"/>
    /// </summary>
    public sealed class ActivitiesProperty
    {
        /// <summary>
        /// Renders current activities.
        /// </summary>
        /// <returns>JSON serialized list of current activities.</returns>
        public override string ToString()
        {
            var activities = Activity.GetCurrentActivities();
            var serializer = ActivityInfoList.DefaultJsonSerializer;

            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, activities);
                var json = Encoding.UTF8.GetString(ms.ToArray());
                return json;
            }
        }
    }
}
