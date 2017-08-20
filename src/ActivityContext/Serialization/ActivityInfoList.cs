using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ActivityContext.Serialization
{
    [CollectionDataContract(Name = Strings.ActivityInfoListElementName, Namespace = Strings.Namespace)]
    public sealed class ActivityInfoList : List<ActivityInfo>
    {
        public static DataContractSerializer DefaultSerializer { get; } = new DataContractSerializer(typeof(ActivityInfoList));

        public static DataContractJsonSerializer DefaultJsonSerializer { get; } = new DataContractJsonSerializer(typeof(ActivityInfoList));
    }
}
