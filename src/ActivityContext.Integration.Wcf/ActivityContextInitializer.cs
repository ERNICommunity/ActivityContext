using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ActivityContext.Data;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Looks for <Activities/> header in message and applies founded activities to the logical thread context.
    /// </summary>
    internal sealed class ActivityContextInitializer : ICallContextInitializer
    {
        /// <summary>
        /// <see cref="ActivityContextInitializer"/> is state-less. Therefore it's safe to share single instance.
        /// </summary>
        public static readonly ActivityContextInitializer DefaultInstance = new ActivityContextInitializer();

        public object BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {
            // Search for header
            var index = message.Headers.FindHeader(ActivityInfoList.ElementName, ActivityInfoList.ElementNamespace);
            if (index == -1)
            {
                return null;
            }

            using (var reader = message.Headers.GetReaderAtHeader(index))
            {
                // De-serialize header
                var activityInfoList = (ActivityInfoList)ActivityInfoList.DefaultSerializer.ReadObject(reader);
                if (activityInfoList.Count == 0)
                {
                    return null;
                }

                // Apply activities to logical context
                return new CompositeActivity(activityInfoList);
            }
        }

        public void AfterInvoke(object correlationState)
        {
            if (correlationState != null)
            {
                var compositeActivity = (CompositeActivity)correlationState;
                compositeActivity.Dispose();
            }
        }
    }
}
