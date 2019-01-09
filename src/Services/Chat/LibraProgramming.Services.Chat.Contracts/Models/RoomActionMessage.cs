using System;
using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Contracts.Models
{
    [Serializable]
    [DataContract]
    public enum RoomAction
    {
        Registered,
        Renamed,
        Removed
    }

    [Serializable]
    [DataContract]
    public class RoomActionMessage
    {
        [DataMember]
        public RoomAction Action
        {
            get;
            set;
        }

        [DataMember]
        public long RoomId
        {
            get;
            set;
        }
    }
}