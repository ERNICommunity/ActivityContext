using System.ServiceModel;
using ActivityContext.Serialization;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// This class should be used as extension data to <see cref="OperationContext"/>.
    /// It holds activity context of remote part (e.g. WCF client).
    /// </summary>
    public sealed class ActivityOperationContext : IExtension<OperationContext>
    {
        public ActivityOperationContext(ActivityInfoList activities)
        {
            Activities = activities;
        }

        public ActivityInfoList Activities { get; }

        void IExtension<OperationContext>.Attach(OperationContext owner)
        {
            // Do nothing.
        }

        void IExtension<OperationContext>.Detach(OperationContext owner)
        {
            // Do nothing.
        }

    }
}
