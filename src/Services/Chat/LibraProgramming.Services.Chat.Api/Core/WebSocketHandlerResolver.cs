using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketHandlerResolver
    {
        private readonly WebSocketHandlerResolverOptions options;
        private readonly IServiceProvider provider;

        public WebSocketHandlerResolver(IOptions<WebSocketHandlerResolverOptions> options, IServiceProvider provider)
        {
            this.options = options.Value;
            this.provider = provider;
        }

        public WebSocketHandler ResolveHandler(PathString path)
        {
            return null;
        }
    }
}