using System;
using Microsoft.AspNetCore.Http;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketHandlerResolverOptions
    {
        private PathString requestPath;

        public PathString RequestPath
        {
            get => requestPath;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                requestPath = value;
            }
        }

        public WebSocketHandlerResolverOptions()
        {
            requestPath = new PathString("/chat");
        }
    }
}