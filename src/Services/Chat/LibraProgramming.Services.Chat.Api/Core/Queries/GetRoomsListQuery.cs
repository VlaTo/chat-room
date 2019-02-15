using System.Collections.Generic;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries
{
    public sealed class GetRoomsListQuery : IRequest<IEnumerable<RoomDescription>>
    {
    }
}