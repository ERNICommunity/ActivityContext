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
        public void MultipleActivitiesWithSameName()
        {
            const string activityName = "Test";
            var activityId1 = Guid.NewGuid();
            var activityId2 = Guid.NewGuid();

            var ac1 = new Activity(activityName, activityId1);
            var ac2 = new Activity(activityName, activityId2);

            var multipleActivities = Activity.GetCurrentActivities();
            Assert.Equal(2, multipleActivities.Count);

            ac1.Dispose();

            var singleActivity = Activity.GetCurrentActivities();
            Assert.Equal(1, singleActivity.Count);
            Assert.Equal(activityId2, singleActivity[0].Id);
        }
    }
}
