using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public abstract class WebSocketHandler
    {
        public virtual Task OnConnectAsync(WebSocket socket)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnMessageAsync(WebSocket socket, WebSocketMessageType messageType, ArraySegment<byte> message)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisconnectAsync(WebSocket socket)
        {
            return Task.CompletedTask;
        }
    }
}