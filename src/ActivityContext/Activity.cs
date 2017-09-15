using System;
using System.Threading;
using ActivityContext.Data;

namespace ActivityContext
{
    /// <summary>
    /// Activity marks logical context of executed code from the point of it's creation until activity is disposed.
    /// After activity is created, it is stored in ambient context which flows through Tasks or Threads.
    /// </summary>
    public sealed class Activity : IDisposable
    {
        /// <summary>
        /// Default name of the activity - "Activity".
        /// </summary>
        public const string DefaultActivityName = "Activity";

        // Contains reference to the lastly created activity in the current context.
        // If no activity was created in the current context,
        // the value reference the lastly created activity in the parent context in the time of creation of the current context.
        private static readonly AsyncLocal<Activity> Context = new AsyncLocal<Activity>();

        /// <summary>
        /// Create new <see cref="Activity"/> in the current context using default name (<see cref="DefaultActivityName"/>).
        /// </summary>
        public Activity()
            : this(DefaultActivityName)
        {
        }

        /// <summary>
        /// Create new <see cref="Activity"/> in the current context.
        /// </summary>
        /// <param name="name">Name of the activity.</param>
        public Activity(string name)
            : this(name, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Create new <see cref="Activity"/> in the current context.
        /// </summary>
        /// <param name="name">Name of the activity.</param>
        /// <param name="id">Globally unique ID of the activity.</param>
        public Activity(string name, Guid id)
        {
            Name = name ?? DefaultActivityName;
            Id = id;

            // Initialize Parent to the latest non-disposed activity in the current context.
            Parent = GetParentActivity();

            // Set itself as the current lastly created activity in the current context.
            Context.Value = this;
        }

        /// <summary>
        /// Name of the activity.
        /// If it's not provided by the user, the <see cref="DefaultActivityName"/> is used.
        /// It is not required to be unique in the current context.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Globally unique identifier of the activity.
        /// If it's not provided by the user, the <see cref="Guid.NewGuid"/> is used.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Reference to previous activity.
        /// Activities in single context are forming a linked list.
        /// However single activity can be referenced also from different context.
        /// To enforce thread safety, references between activities are immutable.
        /// Only mutable state of the activity is flag whether it was already disposed.
        /// </summary>
        public Activity Parent { get; }

        /// <summary>
        /// Flag whether activity has been already disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Removes all activities from the current context.
        /// </summary>
        public static void ClearCurrentContext()
        {
            Context.Value = null;
        }

        /// <summary>
        /// Returns list of active Activities in the current context.
        /// Activity is active when it is not disposed or there is any not disposed child activity.
        /// Activities are in descending order of their creation (most recent is the first).
        /// </summary>
        public static ActivityInfoList GetCurrentActivities()
        {
            var result = new ActivityInfoList();

            var current = Context.Value;

            // Skip completed activities
            while (current != null && current.IsDisposed)
            {
                current = current.Parent;
            }

            while (current != null)
            {
                result.Add(new ActivityInfo { Id = current.Id, Name = current.Name });
                current = current.Parent;
            }

            return result;
        }

        /// <summary>
        /// Flags activity as disposed.
        /// This activity won't be included in the results of future calls
        /// to <see cref="GetCurrentActivities"/> from the current context.
        /// Activity does not hold any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
        }

        // Returns the last active (not disposed) activity from the context.
        private static Activity GetParentActivity()
        {
            var last = Context.Value;
            while (last != null && last.IsDisposed)
            {
                last = last.Parent;
            }

            return last;
        }
    }
}