using System;
using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain
{
    [DataContract]
    public class RoomListOperationResult : OperationResult
    {
        [DataMember]
        public RoomOperationResult[] Rooms
        {
            get;
            set;
        }

        public RoomListOperationResult()
        {
            Rooms = Array.Empty<RoomOperationResult>();
        }
    }
}