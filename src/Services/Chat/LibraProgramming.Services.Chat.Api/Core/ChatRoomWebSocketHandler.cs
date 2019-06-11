using LibraProgramming.ChatRoom.Domain.Messages;
using LibraProgramming.Serialization.Hessian;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private HessianSerializerSettings settings;
        private IAsyncStream<ChatMessage> stream;
        private StreamSubscriptionHandle<ChatMessage> subscription;
        private long roomId;

        public ChatRoomWebSocketHandler(IClusterClient client, IChatRoomRegistry registry, ILogger<ChatRoomWebSocketHandler> logger)
        {
            this.client = client;
            this.registry = registry;
            this.logger = logger;

            encoding = Encoding.UTF8;
            settings = new HessianSerializerSettings
            {
            };
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

        public override Task OnMessageAsync(WebSocket webSocket, WebSocketMessageType messageType, ArraySegment<byte> data)
        {
            if (WebSocketMessageType.Binary != messageType)
            {
                logger.LogDebug($"Unsupported message type: {messageType}");
                return Task.CompletedTask;
            }

            IncomingChatMessage message;

            using (var memoryStream = new MemoryStream(data.Array))
            {
                var serializer = new DataContractHessianSerializer(typeof(IncomingChatMessage), settings);
                message = (IncomingChatMessage) serializer.ReadObject(memoryStream);
            }

            return stream.OnNextAsync(new ChatMessage
            {
                Author = message.Author,
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
            ArraySegment<byte> data;

            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractHessianSerializer(typeof(OutgoingChatMessage), settings);

                serializer.WriteObject(memoryStream, new OutgoingChatMessage
                {
                    Author = message.Author,
                    Content = message.Content,
                    Created = message.Created
                });

                data = new ArraySegment<byte>(memoryStream.ToArray());
            }

            var tasks = new List<Task>();
            var registeredSockets = registry[roomId];
            var sockets = registeredSockets.ToArray();

            sockets.AsParallel().ForAll(socket =>
            {
                try
                {
                    logger.LogDebug($"[ChatRoomWebSocketHandler.OnStreamMessage] Sending data length: {data.Count}");
                    var task = socket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
                    tasks.Add(task);
                }
                catch (WebSocketException exception)
                {
                    Console.WriteLine(exception);
                    registeredSockets.Remove(socket);
                }
            });

            return Task.WhenAll(tasks);
        }
    }
}