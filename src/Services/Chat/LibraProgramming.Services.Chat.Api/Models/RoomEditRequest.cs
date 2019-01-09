using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Models
{
    [DataContract]
    public class RoomEditRequest
    {
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