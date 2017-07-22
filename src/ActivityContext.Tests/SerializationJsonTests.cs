using ActivityContext.Serialization;
using System;
using System.IO;
using System.Linq;
using Xunit;

namespace ActivityContext.Tests
{
    public class SerializationJsonTests
    {
        [Fact]
        public void EmptyList()
        {
            using (var output = new StringWriter())
            {
                ActivityJsonSerializer.Write(Enumerable.Empty<Activity>(), output);
                var json = output.ToString();
                Assert.Equal("[]", json);
            }
        }

        [Fact]
        public void SingleActivity()
        {
            using (var output = new StringWriter())
            {
                var activity = new Activity("Test");
                ActivityJsonSerializer.Write(new[] { activity }, output);
                var json = output.ToString();

                Assert.Equal($"[{ActivityToJson(activity)}]", json);
            }
        }

        [Fact]
        public void MultipleActivities()
        {
            using (var output = new StringWriter())
            {
                var activity1 = new Activity("Test");
                var activity2 = new Activity("Test");

                ActivityJsonSerializer.Write(new[] { activity1, activity2 }, output);
                var json = output.ToString();

                Assert.Equal($"[{ActivityToJson(activity1)},{ActivityToJson(activity2)}]", json);
            }
        }

        [Fact]
        public void ActivityNameIsEscaped()
        {
            using (var output = new StringWriter())
            {
                var name = "\"Test\\";                
                var activity = new Activity(name);
                ActivityJsonSerializer.Write(new[] { activity }, output);
                var json = output.ToString();
                var expected = @"[{""name"":""\""Test\\"",""id"":""" + activity.Id + "\"}]";
                Assert.Equal(expected, json);
            }
        }

        private static string ActivityToJson(Activity activity)
        {
            return $"{{\"name\":\"{activity.Name}\",\"id\":\"{activity.Id}\"}}";
        }
    }
}