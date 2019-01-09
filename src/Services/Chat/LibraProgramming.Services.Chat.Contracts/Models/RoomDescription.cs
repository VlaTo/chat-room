using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Contracts.Models
{
    [DataContract]
    public class RoomDescription
    {
        [DataMember]
        public string Description
        {
            get;
            set;
        }
    }
}