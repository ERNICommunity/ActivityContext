using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Xml;
using ActivityContext.Serialization;

namespace ActivityContext.Integration.Wcf
{
    /// <summary>
    /// Message header which contains information about activity context on the sender side.
    /// Header is serialized into exactly same data format as <see cref="ActivityInfoList"/>.
    /// </summary>
    public sealed class ActivityContextHeader : MessageHeader
    {
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(ActivityInfoList));

        public ActivityContextHeader(ActivityInfoList activities)
        {
            Activities = activities;
        }

        public ActivityInfoList Activities { get; }

        public override string Name => Strings.ActivityInfoListElementName;

        public override string Namespace => Strings.Namespace;

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            Serializer.WriteStartObject(writer, Activities);
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            Serializer.WriteObjectContent(writer, Activities);
        }

        /// <summary>
        /// Tries to read <see cref="ActivityContextHeader"/> from <paramref name="request"/> and return extracted 
        /// <see cref="ActivityInfoList"/>. If header is not present in the message, empty <see cref="ActivityInfoList"/>
        /// is returned.
        /// </summary>        
        public static ActivityInfoList ReadActivities(Message request)
        {
            var index = request.Headers.FindHeader(Strings.ActivityInfoListElementName, Strings.Namespace);
            if (index == -1)
            {
                return new ActivityInfoList();
            }

            using (var reader = request.Headers.GetReaderAtHeader(index))
            {
                return (ActivityInfoList)Serializer.ReadObject(reader);
            }
        }
    }
}