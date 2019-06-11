using System;
using System.IO;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class WebSocketsMiddleware
    {
        private readonly RequestDelegate next;
        private readonly WebSocketHandlerResolver resolver;
        private readonly CancellationToken shutdown;
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="resolver"></param>
        /// <param name="applicationLifetime"></param>
        /// <param name="logger"></param>
        public WebSocketsMiddleware(
            RequestDelegate next, 
            WebSocketHandlerResolver resolver,
            IHostApplicationLifetime applicationLifetime,
            ILogger<WebSocketsMiddleware> logger)
        {
            this.next = next;
            this.resolver = resolver;
            this.logger = logger;
            shutdown = applicationLifetime.ApplicationStopping;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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

        private async Task HandleWebSocket(HttpContext context, WebSocketHandler handler, RouteValueDictionary values)
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await handler.OnConnectAsync(socket, values);

            try
            {
                var cancel = CancellationTokenSource.CreateLinkedTokenSource(shutdown, context.RequestAborted);

                await ReceiveAsync(socket, cancel.Token, async (message, result) =>
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
            catch (Exception exception)
            {
                logger.LogError(exception, "WebSocketsMiddleware.HandleWebSocket");
                throw;
            }
        }

        private static async Task ReceiveAsync(WebSocket socket, CancellationToken cancellationToken, Func<ArraySegment<byte>, WebSocketReceiveResult, Task> handler)
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
                        result = await socket.ReceiveAsync(buffer, cancellationToken);
                        await stream.WriteAsync(buffer.Array, buffer.Offset, result.Count, cancellationToken);
                    }
                    while (false == result.EndOfMessage);

                    cancellationToken.ThrowIfCancellationRequested();

                    message = new ArraySegment<byte>(stream.ToArray());
                }

                await handler.Invoke(message, result);
            }
        }
    }
}