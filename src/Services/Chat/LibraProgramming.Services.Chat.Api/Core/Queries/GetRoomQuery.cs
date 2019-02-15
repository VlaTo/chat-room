using System.Runtime.Serialization;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries
{
    [DataContract]
    public sealed class GetRoomQuery : IRequest<RoomDescription>
    {
        [DataMember(Name = "id")]
        public long Id
        {
            get;
            set;
        }
    }
}