using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain
{
    [DataContract]
    public class RoomOperationResult : OperationResult
    {
        [DataMember]
        public long Id
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Description
        {
            get;
            set;
        }
    }
}