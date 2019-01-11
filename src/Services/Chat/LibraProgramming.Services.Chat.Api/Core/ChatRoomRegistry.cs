using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class ChatRoomRegistry : IChatRoomRegistry
    {
        private readonly IDictionary<long, IList<WebSocket>> channels;

        public IList<WebSocket> this[long roomId]
        {
            get
            {
                if (0 > roomId)
                {
                    throw new ArgumentException("", nameof(roomId));
                }

                if (false == channels.TryGetValue(roomId, out var sockets))
                {
                    sockets = new List<WebSocket>();
                    channels.Add(roomId, sockets);
                }

                return sockets;
            }
        }

        public ChatRoomRegistry()
        {
            channels = new Dictionary<long, IList<WebSocket>>();
        }
    }
}