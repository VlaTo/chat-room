using System.Collections.Generic;
using System.Net.WebSockets;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public interface IChatRoomRegistry
    {
        IList<WebSocket> this[long roomId]
        {
            get;
        }
    }
}