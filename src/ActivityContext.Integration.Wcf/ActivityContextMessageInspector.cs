using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ActivityContext.Integration.Wcf
{
    public class ActivityContextMessageInspector : IDispatchMessageInspector, IClientMessageInspector
    {
        #region IDispatchMessageInspector

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IClientMessageInspector

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            // TODO
            throw new NotImplementedException();
        }

        #endregion
    }
}
