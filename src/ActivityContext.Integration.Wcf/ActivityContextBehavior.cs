using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Enables tracking of the activity context across WCF service calls.
    /// Current context is captured on the client and added to the message headers sent to the service.
    /// Received activity context is applied to the logical thread context before service method implementation is executed.
    /// </summary>
    public sealed class ActivityContextBehavior : Attribute, IServiceBehavior, IEndpointBehavior
    {
        #region IServiceBehavior

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            // Do nothing.
        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
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

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            // Do nothing.
        }

        #endregion

        #region IEndpointBehavior

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            // Do nothing.
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            // Add <Activities/> header to operations invoked on server.
            clientRuntime.ClientMessageInspectors.Add(ActivityContextMessageInspector.DefaultInstance);

            foreach (var op in clientRuntime.CallbackDispatchRuntime.Operations)
            {
                // Initialize Logical context of operation invoked on client callback instance.
                op.CallContextInitializers.Add(ActivityContextInitializer.DefaultInstance);
            }
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // Do nothing.
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
            // Do nothing.
        }

        #endregion
    }
}
