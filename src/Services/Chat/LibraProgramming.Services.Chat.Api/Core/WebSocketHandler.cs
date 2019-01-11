using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public abstract class WebSocketHandler
    {
        public virtual Task OnConnectAsync(WebSocket webSocket, RouteValueDictionary value)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnMessageAsync(WebSocket webSocket, WebSocketMessageType messageType, ArraySegment<byte> message)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisconnectAsync(WebSocket webSocket)
        {
            return Task.CompletedTask;
        }
    }
}