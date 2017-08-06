using System;

namespace ActivityContext
{
    public class ActivityFailedException : Exception
    {
        public ActivityFailedException(Activity activity)
            : base(GetMessage(activity))
        {
            Activity = activity;
        }

        public ActivityFailedException(Activity activity, Exception innerException)
            : base(GetMessage(activity), innerException)
        {
            Activity = activity;
        }

        public ActivityFailedException(string message, Activity activity)
            : base(message)
        {
            Activity = activity;
        }

        public ActivityFailedException(string message, Activity activity, Exception innerException)
            : base(message, innerException)
        {
            Activity = activity;
        }

        public Activity Activity { get; }

        private static string GetMessage(Activity activity) => "Activity failed: " + activity.Name + " {" + activity.Id + "}.";
    }
}
