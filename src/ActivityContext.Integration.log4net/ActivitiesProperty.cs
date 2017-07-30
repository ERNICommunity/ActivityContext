using ActivityContext.Serialization;

namespace ActivityContext.Integration.log4net
{
    public sealed class ActivitiesProperty
    {
        public override string ToString()
        {
            var activities = Activity.GetCurrentActivities();
            return ActivityJsonSerializer.Serialize(activities);
        }
    }
}