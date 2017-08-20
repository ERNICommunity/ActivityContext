using System;
using System.ServiceModel;
using Xunit;

namespace ActivityContext.Integration.Wcf.Tests
{
    public class ClientServerTests
    {
        [Fact]
        public void ClientActivitiesAreReceivedInService()
        {
            var service = new TestService();

            var uri = new Uri("net.pipe://localhost/TestService");

            using (var host = new ServiceHost(service, uri))
            {
                host.Description.Behaviors.Add(new ActivityContextBehavior());
                host.Open();

                var clientFactory = new ChannelFactory<IClientServerServiceClient>(new NetNamedPipeBinding(), new EndpointAddress(uri));
                clientFactory.Endpoint.EndpointBehaviors.Add(new ActivityContextBehavior());
                var client = clientFactory.CreateChannel();

                client.Open();

                using (var activity = new Activity("Test"))
                {
                    client.Invoke(activity.Id, activity.Name);
                }

                client.Close();
                host.Close();
            }
        }
    }

    [ServiceContract]
    public interface IClientServerService
    {
        [OperationContract]
        void Invoke(Guid activityId, string activityName);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class TestService : IClientServerService
    {
        public void Invoke(Guid activityId, string activityName)
        {
            using (OperationContext.Current.ApplyActivityContext())
            {
                var activities = Activity.GetCurrentActivities();
                Assert.Equal(1, activities.Count);
                Assert.Equal(activityName, activities[0].Name);
                Assert.Equal(activityId, activities[0].Id);
            }
        }
    }

    public interface IClientServerServiceClient : IClientServerService, ICommunicationObject
    { }
}