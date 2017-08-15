using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ActivityContext.Integration.Wcf
{
    public sealed class ActivityContextBehavior : IServiceBehavior, IEndpointBehavior
    {
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // TODO
            throw new NotImplementedException();
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // TODO
            throw new NotImplementedException();
        }
    }
}
