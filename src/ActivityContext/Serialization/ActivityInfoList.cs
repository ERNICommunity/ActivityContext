using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ActivityContext.Serialization
{
    [CollectionDataContract(Name = ElementName, Namespace = ElementNamespace)]
    public sealed class ActivityInfoList : List<ActivityInfo>
    {
        public const string ElementName = "Activities";

        public const string ElementNamespace = Strings.ActivityContextNamespace;

        public static DataContractSerializer DefaultSerializer { get; } = new DataContractSerializer(typeof(ActivityInfoList));

        public static DataContractJsonSerializer DefaultJsonSerializer { get; } = new DataContractJsonSerializer(typeof(ActivityInfoList));
    }
}
