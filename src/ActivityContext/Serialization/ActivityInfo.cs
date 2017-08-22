using System;
using System.Runtime.Serialization;

namespace ActivityContext.Serialization
{
    [DataContract(Name = ElementName, Namespace = ElementNamespace)]
    public sealed class ActivityInfo
    {
        public const string ElementName = "Activity";

        public const string ElementNamespace = Strings.ActivityContextNamespace;

        [DataMember(Order = 0)]
        public Guid Id { get; set; }

        [DataMember(Order = 1)]
        public string Name { get; set; }
    }
}
