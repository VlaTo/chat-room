using System;
using System.Runtime.Serialization;
using LibraProgramming.ChatRoom.Domain.Models;

namespace LibraProgramming.ChatRoom.Domain.Results
{
    [DataContract(Name = "result", Namespace = "http://chatroom.org/domain/results/rooms")]
    public sealed class RoomsResult : IApiResult
    {
        [DataMember(Name = "rooms")]
        public RoomDetails[] Rooms
        {
            get;
            set;
        }

        public RoomsResult()
        {
            Rooms = Array.Empty<RoomDetails>();
        }
    }
}