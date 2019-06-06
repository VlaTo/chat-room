using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries
{
    internal sealed class GetRoomQuery : IRequest<RoomDescription>
    {
        public long Id
        {
            get;
        }

        public GetRoomQuery(long id)
        {
            Id = id;
        }
    }
}