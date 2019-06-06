using System.Runtime.Serialization;
using LibraProgramming.ChatRoom.Domain.Models;

namespace LibraProgramming.ChatRoom.Domain.Results
{
    [DataContract(Name = "result", Namespace = "http://chatroom.org/domain/results/room")]
    public class RoomResult : RoomDetails, IApiResult
    {
    }
}