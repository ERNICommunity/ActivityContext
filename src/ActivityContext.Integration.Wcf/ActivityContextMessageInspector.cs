using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Message inspector which integrates activity context into WCF pipeline.
    /// On client side, before the request is send it adds <see cref="ActivityContextHeader"/> with list of current activities.
    /// On server side, after the request is received it reads the <see cref="ActivityContextHeader"/> and
    /// adds <see cref="ActivityOperationContext"/> into <see cref="OperationContext.Extensions"/>.
    /// </summary>
    public sealed class ActivityContextMessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region IDispatchMessageInspector

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var activities = ActivityContextHeader.ReadActivities(request);
            var activityOperationContext = new ActivityOperationContext(activities);
            OperationContext.Current.Extensions.Add(activityOperationContext);

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            // Do nothing.
        }

        #endregion

        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var header = new ActivityContextHeader(Activity.GetCurrentActivities());
            request.Headers.Add(header);

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // Do nothing.
        }

        #endregion
    }
}
