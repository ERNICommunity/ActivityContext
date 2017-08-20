using System;
using Xunit;

namespace ActivityContext.Tests
{
    public class ActivityContextBasicTests
    {
        [Fact]
        public void NoActivities()
        {
            var activities = Activity.GetCurrentActivities();
            Assert.Equal(0, activities.Count);
        }

        [Fact]
        public void SingleActivity()
        {
            const string activityName = "Test";

            using (new Activity(activityName))
            {
                var activities = Activity.GetCurrentActivities();
                Assert.Equal(1, activities.Count);
                Assert.Equal(activityName, activities[0].Name);
            }
        }

        [Fact]
        public void AfterSingleActivity()
        {
            using (new Activity("Test"))
            {
            }

            var activities = Activity.GetCurrentActivities();
            Assert.Equal(0, activities.Count);
        }

        [Fact]
        public void ActivitiesAreInCorrectOrder()
        {
            using (var parent = new Activity("Parent", Guid.NewGuid()))
            using (var child = new Activity("Child", Guid.NewGuid()))
            {
                var activities = Activity.GetCurrentActivities();
                Assert.Equal(2, activities.Count);

                Assert.Equal(child.Id, activities[0].Id);
                Assert.Equal(parent.Id, activities[1].Id);
            }
        }
    }
}
