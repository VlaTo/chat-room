using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Orleans;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Orleans.Streams;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class ChatRoomWebSocketHandler : WebSocketHandler
    {
        private readonly IClusterClient client;
        private readonly IChatRoomRegistry registry;
        private readonly ILogger<ChatRoomWebSocketHandler> logger;
        private readonly Encoding encoding;
        private WebSocket socket;
        private IAsyncStream<ChatMessage> stream;
        private StreamSubscriptionHandle<ChatMessage> subscription;
        private long roomId;

        public ChatRoomWebSocketHandler(IClusterClient client, IChatRoomRegistry registry, ILogger<ChatRoomWebSocketHandler> logger)
        {
            this.client = client;
            this.registry = registry;
            this.logger = logger;
            encoding = Encoding.UTF8;
        }

        public override async Task OnConnectAsync(WebSocket webSocket, RouteValueDictionary values)
        {
            var room = Convert.ToInt64(values["room"]);
            var streamProvider = client.GetStreamProvider(Constants.StreamProviders.ChatRooms);
            var chat = client.GetGrain<IChatRoom>(room);
            var streamId = await chat.JoinAsync();

            socket = webSocket;
            roomId = room;
            stream = streamProvider.GetStream<ChatMessage>(streamId, Constants.Streams.Namespaces.Chats);
            subscription = await stream.SubscribeAsync(OnMessage);

            registry[room].Add(webSocket);
        }

        public override Task OnMessageAsync(WebSocket webSocket, WebSocketMessageType messageType,
            ArraySegment<byte> message)
        {
            if (WebSocketMessageType.Text != messageType)
            {
                logger.LogDebug($"Unsupported message type: {messageType}");
                return Task.CompletedTask;
            }

            var text = encoding.GetString(message.Array);

            return stream.OnNextAsync(new ChatMessage
            {
                Content = text,
                Created = DateTime.UtcNow
            });
        }

        public override async Task OnDisconnectAsync(WebSocket webSocket)
        {
            registry[roomId].Remove(socket);
            await subscription.UnsubscribeAsync();
        }

        private Task OnMessage(ChatMessage message, StreamSequenceToken sst)
        {
            logger.LogInformation($"Message from stream: \"{message.Content}\"");

            var bytes = encoding.GetBytes(message.Content);
            var data = new ArraySegment<byte>(bytes);
            var tasks = new List<Task>();
            var sockets = registry[roomId];

            foreach (var webSocket in sockets)
            {
                var task = webSocket.SendAsync(data, WebSocketMessageType.Text, true, CancellationToken.None);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }
    }
}