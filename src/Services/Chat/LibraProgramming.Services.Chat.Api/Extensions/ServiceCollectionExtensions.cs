using System;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddChatRoomHandler(this IServiceCollection services,
            Action<ChatRoomOptions> configurator)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (null == configurator)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            services
                .TryAddEnumerable(
                    ServiceDescriptor.Transient<IConfigureOptions<ChatRoomOptions>, ChatRoomOptionsSetup>()
                );

            services.TryAddTransient<WebSocketHandler, ChatRoomWebSocketHandler>();
            services.Configure(configurator);

            return services;
        }
    }
}