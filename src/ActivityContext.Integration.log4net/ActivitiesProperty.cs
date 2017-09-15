using System.IO;
using System.Text;
using ActivityContext.Data;

namespace ActivityContext.Integration.log4net
{
    public sealed class ActivitiesProperty
    {
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
