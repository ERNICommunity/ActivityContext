using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Adds <see cref="ActivityContextMessageInspector"/> to clients and services.
    /// </summary>
    public sealed class ActivityContextBehavior : Attribute, IServiceBehavior, IEndpointBehavior
    {
        #region IServiceBehavior

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // Do nothing.
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    // Add <Activities/> header to operations invoked on callback channel.
                    endpointDispatcher.DispatchRuntime.CallbackClientRuntime.MessageInspectors.Add(ActivityContextMessageInspector.DefaultInstance);

                    foreach (var op in endpointDispatcher.DispatchRuntime.Operations)
                    {
                        // Initialize Logical context of operation invocation.
                        op.CallContextInitializers.Add(ActivityContextInitializer.DefaultInstance);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Do nothing.
        }

        #endregion

        #region IEndpointBehavior

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // Do nothing.
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // Add <Activities/> header to operations invoked on server.
            clientRuntime.ClientMessageInspectors.Add(ActivityContextMessageInspector.DefaultInstance);

            foreach (var op in clientRuntime.CallbackDispatchRuntime.Operations)
            {
                // Initialize Logical context of operation invoked on client callback instance.
                op.CallContextInitializers.Add(ActivityContextInitializer.DefaultInstance);
            }
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Do nothing.
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // Do nothing.
        }

        #endregion
    }
}
