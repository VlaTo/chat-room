using System;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketHandlers(this IServiceCollection services,
            Action<WebSocketHandlerResolverOptions> configurator)
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
                    ServiceDescriptor.Transient<IConfigureOptions<WebSocketHandlerResolverOptions>, WebSocketHandlerResolverOptionsSetup>()
                );

            services.TryAddTransient<WebSocketHandlerResolver>();
            services.Configure(configurator);

            return services;
        }
    }
}