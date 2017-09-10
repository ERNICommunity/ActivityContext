using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Xunit;

namespace ActivityContext.Integration.Wcf.Tests
{
    public class DuplexTests
    {
        [Fact]
        public async Task ServerActivitiesAreReceivedInClient()
        {
            var service = new TestService();
            var clientService = new TestClient();

            var uri = new Uri("net.pipe://localhost/DuplexTests/TestService");

            using (var host = new ServiceHost(service, uri))
            {
                host.Description.Behaviors.Add(new ActivityContextBehavior());
                host.Open();

                await Task.Run(() =>
                {
                    var clientFactory = new DuplexChannelFactory<IDuplexServiceClient>(clientService, new NetNamedPipeBinding(), new EndpointAddress(uri));
                    clientFactory.Endpoint.EndpointBehaviors.Add(new ActivityContextBehavior());
                    var client = clientFactory.CreateChannel();

                    client.Open();
                    client.Init();

                    using (var activity = new Activity("Test"))
                    {
                        service.Client.Invoke(activity.Id, activity.Name);
                    }

                    client.Close();
                });

                host.Close();
            }
        }

        [ServiceContract(CallbackContract = (typeof(IDuplexServiceCallback)))]
        public interface IDuplexService
        {
            [OperationContract]
            void Init();
        }

        public interface IDuplexServiceCallback
        {
            [OperationContract]
            void Invoke(Guid activityId, string activityName);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class TestService : IDuplexService
        {
            public void Init()
            {
                Client = OperationContext.Current.GetCallbackChannel<IDuplexServiceCallback>();
            }

            public IDuplexServiceCallback Client { get; set; }
        }

        public class TestClient : IDuplexServiceCallback
        {
            public void Invoke(Guid activityId, string activityName)
            {
                var activities = Activity.GetCurrentActivities();
                Assert.Equal(1, activities.Count);
                Assert.Equal(activityName, activities[0].Name);
                Assert.Equal(activityId, activities[0].Id);
            }
        }

        public interface IDuplexServiceClient : IDuplexService, ICommunicationObject
        { }
    }
}
