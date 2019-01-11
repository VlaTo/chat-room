using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly WebSocketHandlerResolver resolver;

        public WebSocketsMiddleware(RequestDelegate next, WebSocketHandlerResolver resolver)
        {
            this.next = next;
            this.resolver = resolver;
        }

        public Task Invoke(HttpContext context)
        {
            if (false == context.WebSockets.IsWebSocketRequest)
            {
                return next.Invoke(context);
            }

            var values = new RouteValueDictionary();
            var handler = resolver.ResolveHandler(context.Request.Path, values);

            return null == handler ? next.Invoke(context) : HandleWebSocket(context, handler, values);
        }

        private static async Task HandleWebSocket(HttpContext context, WebSocketHandler handler, RouteValueDictionary values)
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await handler.OnConnectAsync(socket, values);

            try
            {
                await ReceiveAsync(socket, context.RequestAborted, async (message, result) =>
                {
                    switch (result.MessageType)
                    {
                        case WebSocketMessageType.Binary:
                        case WebSocketMessageType.Text:
                        {
                            await handler.OnMessageAsync(socket, result.MessageType, message);
                            break;
                        }

                        case WebSocketMessageType.Close:
                        {
                            await handler.OnDisconnectAsync(socket);
                            break;
                        }

                        default:
                        {
                            throw new InvalidDataException();
                        }
                    }
                });
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private static async Task ReceiveAsync(WebSocket socket, CancellationToken ct, Func<ArraySegment<byte>, WebSocketReceiveResult, Task> handler)
        {
            while (WebSocketState.Open == socket.State)
            {
                ArraySegment<byte> message;
                WebSocketReceiveResult result;

                using (var stream = new MemoryStream())
                {
                    var buffer = new ArraySegment<byte>(new byte[8192]);

                    do
                    {
                        result = await socket.ReceiveAsync(buffer, ct);
                        await stream.WriteAsync(buffer.Array, buffer.Offset, result.Count, ct);
                    }
                    while (false == result.EndOfMessage);

                    ct.ThrowIfCancellationRequested();

                    message = new ArraySegment<byte>(stream.ToArray());
                }

                await handler.Invoke(message, result);
            }
        }
    }
}