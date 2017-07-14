using System;
using System.Threading.Tasks;
using Xunit;

namespace ActivityContext.Tests
{
    public class ActivityContextTasksTests
    {
        /// <summary>
        /// Main activity covers execution of two child concurrent tasks.
        /// Tasks executes code, which enforces overlap of their execution.
        /// First child task creates activity scope. Tests verifies that this activity is not visible in second child task.
        /// </summary>
        [Fact]
        public async Task ConcurrentTasksDoNotAffectEachOtherContext()
        {
            const string activityName = "Test";
            var activityId1 = Guid.NewGuid();
            var activityId2 = Guid.NewGuid();

            var gate1 = new TaskCompletionSource<bool>();
            var gate2 = new TaskCompletionSource<bool>();

            using (ActivityContext.Activity(activityName, activityId1))
            {
                var t1 = Task.Run(async () =>
                {
                    using (ActivityContext.Activity(activityName, activityId2))
                    {
                        // Flag activity scope is created
                        gate1.SetResult(true);

                        // Wait for asserts in second task
                        await gate2.Task;
                    }
                });

                var t2 = Task.Run(async () =>
                {
                    // Wait for activity scope creation in first task
                    await gate1.Task;

                    // Assert only the parent activity is visible here.
                    var activities = ActivityContext.GetCurrentActivities();
                    Assert.Equal(1, activities.Length);
                    Assert.Equal(activityId1, activities[0].Value);

                    // Flag assertion are completed
                    gate2.SetResult(true);
                });

                // Wait for completion of both tasks
                await t1;
                await t2;
            }
        }
    }
}
