using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orleans;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class ChatRoomWebSocketHandler : WebSocketHandler
    {
        private readonly IClusterClient client;
        private readonly WebSocketHandlerResolverOptions options;
        private readonly ILogger<ChatRoomWebSocketHandler> logger;
        private WebSocket socket;
        private IAsyncStream<ChatMessage> stream;
        private StreamSubscriptionHandle<ChatMessage> subscription;
        private long roomId;

        public ChatRoomWebSocketHandler(IClusterClient client, IOptions<WebSocketHandlerResolverOptions> options, ILogger<ChatRoomWebSocketHandler> logger)
        {
            this.client = client;
            this.options = options.Value;
            this.logger = logger;
        }

        /*public override bool CanAccept(HttpRequest request)
        {
            if (false == request.Path.StartsWithSegments(options.RequestPath))
            {
                return false;
            }

            var optionsPath = options.RequestPath.ToString();
            var tail = request.Path.ToString().Substring(optionsPath.Length);

            while (tail.StartsWith('/') || tail.StartsWith('\\'))
            {
                tail = tail.Substring(1);
            }

            roomId = Convert.ToInt64(tail);

            return true;
        }*/

        public override async Task OnConnectAsync(WebSocket s)
        {
            socket = s;

            var streamProvider = client.GetStreamProvider(Constants.StreamProviders.ChatRooms);
            var room = client.GetGrain<IChatRoom>(roomId);
            var streamId = await room.JoinAsync();

            stream = streamProvider.GetStream<ChatMessage>(streamId, Constants.Streams.Namespaces.Chats);
            subscription = await stream.SubscribeAsync(OnMessage);
        }

        public override Task OnMessageAsync(WebSocket s, WebSocketMessageType messageType,ArraySegment<byte> message)
        {
            var text = Encoding.UTF8.GetString(message.Array);

            return stream.OnNextAsync(new ChatMessage
            {
                Content = text,
                Created = DateTime.UtcNow
            });
        }

        public override async Task OnDisconnectAsync(WebSocket s)
        {
            await subscription.UnsubscribeAsync();
        }

        private Task OnMessage(ChatMessage message, StreamSequenceToken t)
        {
            logger.LogInformation($"Message from stream: \"{message.Content}\"");

            var bytes = Encoding.UTF8.GetBytes(message.Content);
            var data = new ArraySegment<byte>(bytes);

            return socket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}