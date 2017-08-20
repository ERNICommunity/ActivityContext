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

            using (new Activity(activityName, activityId1))
            {
                var t1 = Task.Run(async () =>
                {
                    using (new Activity(activityName, activityId2))
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
                    var activities = Activity.GetCurrentActivities();
                    Assert.Equal(1, activities.Count);
                    Assert.Equal(activityId1, activities[0].Id);

                    // Flag assertion are completed
                    gate2.SetResult(true);
                });

                // Wait for completion of both tasks
                await t1;
                await t2;
            }
        }

        [Fact]
        public async Task ActivityCanBeDisposedInDifferentContext()
        {
            var activity = new Activity();
            await Task.Run(() => activity.Dispose());

            var activities = Activity.GetCurrentActivities();
            Assert.Equal(0, activities.Count);
        }

        [Fact]
        public async Task DisposedActivityIsPreservedInChildContext()
        {
            var gate1 = new TaskCompletionSource<bool>();
            var gate2 = new TaskCompletionSource<bool>();

            Task task;

            using (new Activity("Main"))
            {
                task = Task.Run(async () =>
                {
                    using (new Activity("Child"))
                    {
                        // Flag Child activity is created.
                        gate1.SetResult(true);

                        // Wait until main activity is disposed.
                        await gate2.Task;

                        // Main activity is already disposed, however child activity is still active.
                        // Therefore current context should contain both activities.
                        Assert.Equal(2, Activity.GetCurrentActivities().Count);
                    }
                });

                // Wait until child activity is created.
                await gate1.Task;
            }

            // Main activity is already disposed.
            Assert.Equal(0, Activity.GetCurrentActivities().Count);

            gate2.SetResult(true);
            await task;
        }
    }
}
