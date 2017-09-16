using System;
using System.Collections.Generic;
using ActivityContext.Data;

namespace ActivityContext
{
    /// <summary>
    /// Represents group of activities which should be applied and disposed together.
    /// Main use case is to apply serialized activity context received from remote partner (e.g. WCF client).
    /// </summary>
    public sealed class CompositeActivity : IDisposable
    {
        private readonly List<Activity> _activities;

        /// <summary>
        /// Constructs new <see cref="CompositeActivity"/>. Activities defined in <paramref name="activityInfoList"/> are applied to the current context in the reverse order.
        /// </summary>
        /// <param name="activityInfoList"></param>
        public CompositeActivity(ActivityInfoList activityInfoList)
        {
            _activities = new List<Activity>(activityInfoList.Count);

            // ActivityInfoList should contain activities in descending order of their creation.
            // Therefore we have to apply them in reverse order.
            for (int i = activityInfoList.Count - 1; i >= 0; i--)
            {
                var activityInfo = activityInfoList[i];
                var activity = new Activity(activityInfo.Name, activityInfo.Id);
                _activities.Add(activity);
            }
        }

        /// <summary>
        /// Calls <see cref="Dispose"/> on each activity.
        /// </summary>
        public void Dispose()
        {
            foreach (var activity in _activities)
            {
                activity.Dispose();
            }
        }
    }
}
