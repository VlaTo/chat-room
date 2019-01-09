using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public sealed class ChatRoomOptionsSetup : ConfigureOptions<ChatRoomOptions>
    {
        public ChatRoomOptionsSetup(IOptions<WebSocketOptions> webSocketOptions)
            : base(options => ConfigureOptions(options, webSocketOptions.Value))
        {
        }

        public static void ConfigureOptions(ChatRoomOptions options, WebSocketOptions socketOptions)
        {
            ;
        }
    }
}