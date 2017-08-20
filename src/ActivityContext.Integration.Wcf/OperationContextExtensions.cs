using System.ServiceModel;
using ActivityContext.Serialization;

namespace ActivityContext.Integration.Wcf
{
    public static class OperationContextExtensions
    {
        /// <summary>
        /// Tries to find <see cref="ActivityOperationContext"/> in <paramref name="context"/> extensions
        /// and apply it. If extension is not found, empty composite activity will be returned.
        /// Do not forget to dispose returned <see cref="CompositeActivity"/>.
        /// </summary>        
        public static CompositeActivity ApplyActivityContext(this OperationContext @this)
        {
            var ctx = @this.GetActivityContext();
            var activities = ctx?.Activities ?? new ActivityInfoList();

            return new CompositeActivity(activities);
        }


        public static ActivityOperationContext GetActivityContext(this OperationContext @this)
        {
            return @this.Extensions.Find<ActivityOperationContext>();

        }
    }
}
