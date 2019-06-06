using System.Collections.Generic;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries
{
    internal sealed class GetRoomsQuery : IRequest<IEnumerable<RoomDescription>>
    {
    }
}