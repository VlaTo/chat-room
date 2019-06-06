using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Domain.Models
{
    [DataContract(Name = "room", Namespace = "http://chatroom.org/domain/models/room")]
    public class RoomDetails
    {
        [DataMember(Name = "id")]
        public long Id
        {
            get;
            set;
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        [DataMember(Name = "description")]
        public string Description
        {
            get;
            set;
        }
    }
}