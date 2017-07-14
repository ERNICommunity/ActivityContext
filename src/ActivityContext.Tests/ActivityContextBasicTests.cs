using System;
using Xunit;

namespace ActivityContext.Tests
{
    public class ActivityContextBasicTests
    {
        [Fact]
        public void NoActivities()
        {
            var activities = ActivityContext.GetCurrentActivities();
            Assert.Equal(0, activities.Length);
        }

        [Fact]
        public void SingleActivity()
        {
            const string activityName = "Test";

            using (ActivityContext.Activity(activityName))
            {
                var activities = ActivityContext.GetCurrentActivities();
                Assert.Equal(1, activities.Length);
                Assert.Equal(activityName, activities[0].Key);
            }
        }

        [Fact]
        public void AfterSingleActivity()
        {
            using (ActivityContext.Activity("Test"))
            {
            }

            var activities = ActivityContext.GetCurrentActivities();
            Assert.Equal(0, activities.Length);
        }

        [Fact]
        public void MultipleActivitiesWithSameName()
        {
            const string activityName = "Test";
            var activityId1 = Guid.NewGuid();
            var activityId2 = Guid.NewGuid();

            var ac1 = ActivityContext.Activity(activityName, activityId1);
            var ac2 = ActivityContext.Activity(activityName, activityId2);

            var multipleActivities = ActivityContext.GetCurrentActivities();
            Assert.Equal(2, multipleActivities.Length);

            ac1.Dispose();

            var singleActivity = ActivityContext.GetCurrentActivities();
            Assert.Equal(1, singleActivity.Length);
            Assert.Equal(activityId2, singleActivity[0].Value);
        }
    }
}
