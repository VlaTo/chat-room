using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public abstract class WebSocketHandler
    {
        public abstract bool CanAccept(HttpRequest request);

        public virtual Task OnConnectAsync(WebSocket socket)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnBinaryAsync(WebSocket socket, ArraySegment<byte> message)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnTextAsync(WebSocket socket, string text)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnDisconnectAsync(WebSocket socket)
        {
            return Task.CompletedTask;
        }
    }
}