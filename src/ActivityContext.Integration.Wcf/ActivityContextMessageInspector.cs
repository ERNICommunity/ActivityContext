using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using ActivityContext.Serialization;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Message inspector which adds <see cref="ActivityContextHeader"/> with list of current activities to the message headers.
    /// </summary>
    internal sealed class ActivityContextMessageInspector : IClientMessageInspector
    {
        /// <summary>
        /// <see cref="ActivityContextMessageInspector"/> is state-less. Therefore it's safe to share single instance.
        /// </summary>
        public static ActivityContextMessageInspector DefaultInstance = new ActivityContextMessageInspector();

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var activityInfoList = Activity.GetCurrentActivities();

            if (activityInfoList.Count > 0)
            {
                var header = MessageHeader.CreateHeader(ActivityInfoList.ElementName, ActivityInfoList.ElementNamespace, activityInfoList, ActivityInfoList.DefaultSerializer);
                request.Headers.Add(header);
            }

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Do nothing.
        }
    }
}
