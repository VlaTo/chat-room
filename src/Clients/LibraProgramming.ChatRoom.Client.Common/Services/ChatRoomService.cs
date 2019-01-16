using LibraProgramming.Services.Chat.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Common.Hessian;
using LibraProgramming.Services.Chat.Domain.Messages;

namespace LibraProgramming.ChatRoom.Client.Common.Services
{
    public sealed class ChatRoomService : IChatRoomService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<IReadOnlyList<Models.ChatRoom>> GetRoomsAsync(CancellationToken ct)
        {
            try
            {
                var request = WebRequest.Create("http://192.168.1.127:5000/api/rooms/");

                request.Method = WebRequestMethods.Http.Get;

                using (var response = await request.GetResponseAsync())
                {
                    var http = (HttpWebResponse) response;

                    if (HttpStatusCode.OK != http.StatusCode)
                    {
                        return null;
                    }

                    var list = new List<Models.ChatRoom>();

                    using (var stream = response.GetResponseStream())
                    {
                        var streamReader = new StreamReader(stream);

                        using (var reader = new JsonTextReader(streamReader))
                        {
                            var serializer = new JsonSerializer();
                            var result = serializer.Deserialize<RoomListOperationResult>(reader);

                            foreach (var room in result.Rooms)
                            {
                                var model = new Models.ChatRoom
                                {
                                    Id = room.Id,
                                    Title = room.Name,
                                    Description = room.Description
                                };

                                list.Add(model);
                            }
                        }
                    }

                    return new ReadOnlyCollection<Models.ChatRoom>(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="room"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<Models.ChatRoom> SaveRoomAsync(string name, string description, CancellationToken ct)
        {
            try
            {
                var request = (HttpWebRequest) WebRequest.Create("http://192.168.1.127:5000/api/room/");

                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/json";

                using (var stream = await request.GetRequestStreamAsync())
                {
                    var writer = new StreamWriter(stream);

                    using (var w = new JsonTextWriter(writer))
                    {
                        var serializer = new JsonSerializer();
                        var room = new {Name = name, Description = description};

                        serializer.Serialize(w, room);
                    }
                }

                using (var response = await request.GetResponseAsync())
                {
                    var http = (HttpWebResponse)response;

                    if (HttpStatusCode.OK != http.StatusCode)
                    {
                        return null;
                    }

                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream);

                        using (var r = new JsonTextReader(reader))
                        {
                            var serializer = new JsonSerializer();
                            var result = serializer.Deserialize<RoomOperationResult>(r);

                            return new Models.ChatRoom
                            {
                                Id = result.Id,
                                Title = result.Name,
                                Description = result.Description
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<IChatChannel> OpenChatAsync(long roomId, CancellationToken ct)
        {
            var socket = new ClientWebSocket();

            await socket.ConnectAsync(new Uri($"ws://192.168.1.127:5000/api/chat/{roomId}"), ct);

            var channel = new ChatChannel(this, socket);

            channel.StartReceive();

            return channel;
        }

        /// <summary>
        /// 
        /// </summary>
        private class ChatChannel : IChatChannel
        {
            private readonly ChatRoomService service;
            private readonly ClientWebSocket socket;
            private readonly DataContractHessianSerializer incomingSerializer;
            private readonly DataContractHessianSerializer outgoingSerializer;
            private readonly CancellationTokenSource cts;
            private Task receive;
            private bool disposed;

            public event EventHandler<ChatMessageEventArgs> MessageArrived;

            public ChatChannel(ChatRoomService service, ClientWebSocket socket)
            {
                this.service = service;
                this.socket = socket;

                var settings = new HessianSerializerSettings
                {
                };

                cts = new CancellationTokenSource();
                incomingSerializer = new DataContractHessianSerializer(typeof(IncomingChatMessage), settings);
                outgoingSerializer = new DataContractHessianSerializer(typeof(OutgoingChatMessage), settings);
            }

            public Task SendAsync(string text)
            {
                ArraySegment<byte> data;

                using (var stream = new MemoryStream())
                {
                    incomingSerializer.WriteObject(stream, new IncomingChatMessage
                    {
                        Author = String.Empty,
                        Content = text
                    });
                    data = new ArraySegment<byte>(stream.ToArray());
                }

                return socket.SendAsync(data, WebSocketMessageType.Binary, true, CancellationToken.None);
            }

            public void StartReceive()
            {
                var ct = cts.Token;

                receive = Task.Run(async () =>
                {
                    while (WebSocketState.Open == socket.State)
                    {
                        ArraySegment<byte> data;
                        WebSocketReceiveResult result;

                        using (var stream = new MemoryStream())
                        {
                            var buffer = new ArraySegment<byte>(new byte[8192]);

                            do
                            {
                                result = await socket.ReceiveAsync(buffer, ct);
                                await stream.WriteAsync(buffer.Array, buffer.Offset, result.Count, ct);
                            } while (false == result.EndOfMessage);

                            ct.ThrowIfCancellationRequested();

                            data = new ArraySegment<byte>(stream.ToArray());
                        }

                        if (0 < data.Count && WebSocketMessageType.Binary == result.MessageType)
                        {
                            OutgoingChatMessage message;
                            
                            using (var stream = new MemoryStream(data.Array))
                            {
                                message = (OutgoingChatMessage) outgoingSerializer.ReadObject(stream);
                            }

                            MessageArrived?.Invoke(this, new ChatMessageEventArgs(message));
                        }
                    }
                }, ct);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        cts.Cancel();
                        socket.Dispose();

                        //Task.WaitAll(receive);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}