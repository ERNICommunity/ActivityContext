using System;
using System.Runtime.Serialization;

namespace ActivityContext.Serialization
{
    [DataContract(Name = Strings.ActivityInfoElementName, Namespace = Strings.Namespace)]
    public sealed class ActivityInfo
    {
        [DataMember(Order = 0)]
        public Guid Id { get; set; }

        [DataMember(Order = 1)]
        public string Name { get; set; }
    }
}
