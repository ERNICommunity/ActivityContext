using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using ActivityContext.Data;

namespace ActivityContext
{
    /// <summary>
    /// The exception should be used as a wrapped around any error that happened in logical piece of code scoped under some <see cref="Activity"/>.
    /// Purpose is to propagate short simple information that some specific named activity failed.
    /// Original error may be included as <see cref="Exception.InnerException"/>.
    /// </summary>
    [Serializable]
    public class ActivityFailedException : Exception
    {
        /// <summary>
        /// Constructs new <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="activity">Activity which failed.</param>
        public ActivityFailedException(Activity activity)
            : base(GetMessage(activity))
        {
            Activity = activity.ToActivityInfo();
        }

        /// <summary>
        /// Constructs new <see cref="ActivityFailedException"/>.
        /// Message is composed using pattern: 'Activity failed: {ActivityName} {ActivityId}'.
        /// </summary>
        /// <param name="activity">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ActivityFailedException(Activity activity, Exception innerException)
            : base(GetMessage(activity), innerException)
        {
            Activity = activity.ToActivityInfo();
        }

        /// <summary>
        /// Constructs new <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="activity">Activity which failed.</param>
        public ActivityFailedException(string message, Activity activity)
            : base(message)
        {
            Activity = activity.ToActivityInfo();
        }

        /// <summary>
        /// Constructs new <see cref="ActivityFailedException"/>.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="activity">Activity which failed.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public ActivityFailedException(string message, Activity activity, Exception innerException)
            : base(message, innerException)
        {
            Activity = activity.ToActivityInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityFailedException"/> class with serialized data.
        /// </summary>        
        protected ActivityFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Activity = new ActivityInfo
            {
                Id = (Guid)info.GetValue(nameof(Activity.Id), typeof(Guid)),
                Name = (string)info.GetValue(nameof(Activity.Name), typeof(string))
            };
        }

        /// <summary>
        /// Sets the System.Runtime.Serialization.SerializationInfo with information about the exception.
        /// </summary>        
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(nameof(Activity.Id), Activity.Id);
            info.AddValue(nameof(Activity.Name), Activity.Name);
        }

        /// <summary>
        /// The <see cref="Activity"/> which failed.
        /// </summary>
        public ActivityInfo Activity { get; }

        private static string GetMessage(Activity activity) => "Activity failed: " + activity.Name + " {" + activity.Id + "}.";
    }
}
