using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class RoomResolveResponse
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