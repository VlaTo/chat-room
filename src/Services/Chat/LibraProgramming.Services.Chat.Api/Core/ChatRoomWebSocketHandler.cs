using LibraProgramming.Serialization.Hessian;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using LibraProgramming.Services.Chat.Domain.Messages;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class ChatRoomWebSocketHandler : WebSocketHandler
    {
        private readonly IClusterClient client;
        private readonly IChatRoomRegistry registry;
        private readonly ILogger<ChatRoomWebSocketHandler> logger;
        private readonly Encoding encoding;
        private readonly DataContractHessianSerializer incomingSerializer;
        private readonly DataContractHessianSerializer outgoingSerializer;
        private IAsyncStream<ChatMessage> stream;
        private StreamSubscriptionHandle<ChatMessage> subscription;
        private long roomId;

        public ChatRoomWebSocketHandler(IClusterClient client, IChatRoomRegistry registry, ILogger<ChatRoomWebSocketHandler> logger)
        {
            this.client = client;
            this.registry = registry;
            this.logger = logger;

            var settings = new HessianSerializerSettings
            {

            };

            encoding = Encoding.UTF8;
            incomingSerializer = new DataContractHessianSerializer(typeof(IncomingChatMessage), settings);
            outgoingSerializer = new DataContractHessianSerializer(typeof(OutgoingChatMessage), settings);
        }

        public override async Task OnConnectAsync(WebSocket webSocket, RouteValueDictionary values)
        {
            var room = Convert.ToInt64(values["room"]);
            var streamProvider = client.GetStreamProvider(Constants.StreamProviders.ChatRooms);
            var chat = client.GetGrain<IChatRoom>(room);
            var streamId = await chat.JoinAsync();

            roomId = room;
            stream = streamProvider.GetStream<ChatMessage>(streamId, Constants.Streams.Namespaces.Chats);

            var handlers = await stream.GetAllSubscriptionHandles();

            if (null == handlers || 0 == handlers.Count)
            {
                subscription = await stream.SubscribeAsync(OnStreamMessage);
            }
            else
            {
                foreach (var handler in handlers)
                {
                    subscription = await handler.ResumeAsync(OnStreamMessage);
                }
            }

            registry[room].Add(webSocket);
        }

        public override Task OnMessageAsync(WebSocket webSocket, WebSocketMessageType messageType,
            ArraySegment<byte> data)
        {
            if (WebSocketMessageType.Binary != messageType)
            {
                logger.LogDebug($"Unsupported message type: {messageType}");
                return Task.CompletedTask;
            }

            IncomingChatMessage message;

            using (var memoryStream = new MemoryStream(data.Array))
            {
                message = (IncomingChatMessage) incomingSerializer.ReadObject(memoryStream);
            }

            return stream.OnNextAsync(new ChatMessage
            {
                Content = message.Content,
                Created = DateTime.UtcNow
            });
        }

        public override Task OnDisconnectAsync(WebSocket webSocket)
        {
            registry[roomId].Remove(webSocket);
            return subscription.UnsubscribeAsync();
        }

        private Task OnStreamMessage(ChatMessage message, StreamSequenceToken sst)
        {
            logger.LogDebug($"Message from stream: \"{message.Content}\"");

            ArraySegment<byte> data;
            var tasks = new List<Task>();
            var sockets = registry[roomId];

            using (var memoryStream = new MemoryStream())
            {
                outgoingSerializer.WriteObject(memoryStream, new OutgoingChatMessage
                {
                    Author = String.Empty,
                    Content = message.Content,
                    Created = message.Created
                });

                data = new ArraySegment<byte>(memoryStream.ToArray());
            }

            foreach (var webSocket in sockets)
            {
                var task = webSocket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
                tasks.Add(task);
            }

            return Task.WhenAll(tasks);
        }
    }
}