using System;
using System.Collections.Generic;
using ActivityContext.Serialization;

namespace ActivityContext
{
    /// <summary>
    /// Represents group of activities which should be applied and disposed together.
    /// Main usecase is to apply serialized activity context received from remote partner (e.g. WCF client).
    /// </summary>
    public sealed class CompositeActivity : IDisposable
    {
        public readonly List<Activity> _activities;

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

        public void Dispose()
        {
            foreach (var activity in _activities)
            {
                activity.Dispose();
            }
        }
    }
}
