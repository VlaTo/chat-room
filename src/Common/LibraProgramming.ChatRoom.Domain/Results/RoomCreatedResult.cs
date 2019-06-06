using System.Runtime.Serialization;
using LibraProgramming.ChatRoom.Domain.Models;

namespace LibraProgramming.ChatRoom.Domain.Results
{
    [DataContract(Name = "result", Namespace = "http://chatroom.org/domain/results/roomcreated")]
    public sealed class RoomCreatedResult : RoomDetails, IApiResult
    {
    }
}