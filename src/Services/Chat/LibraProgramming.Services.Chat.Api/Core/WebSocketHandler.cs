using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public abstract class WebSocketHandler
    {
        public abstract Task OnConnectAsync(WebSocket webSocket, RouteValueDictionary value);

        public abstract Task OnMessageAsync(WebSocket webSocket, WebSocketMessageType messageType, ArraySegment<byte> data);

        public abstract Task OnDisconnectAsync(WebSocket webSocket);
    }
}