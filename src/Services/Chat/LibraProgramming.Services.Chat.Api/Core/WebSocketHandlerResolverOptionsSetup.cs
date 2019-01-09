using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public sealed class WebSocketHandlerResolverOptionsSetup : ConfigureOptions<WebSocketHandlerResolverOptions>
    {
        public WebSocketHandlerResolverOptionsSetup(IOptions<WebSocketOptions> webSocketOptions)
            : base(options => ConfigureOptions(options, webSocketOptions.Value))
        {
        }

        public static void ConfigureOptions(WebSocketHandlerResolverOptions options, WebSocketOptions socketOptions)
        {
            ;
        }
    }
}