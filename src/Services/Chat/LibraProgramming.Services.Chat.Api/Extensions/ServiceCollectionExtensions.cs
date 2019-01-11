using System;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketHandlers(this IServiceCollection services,
            Action<WebSocketHandlersBuilder> configurator)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (null == configurator)
            {
                throw new ArgumentNullException(nameof(configurator));
            }

            var builder = new WebSocketHandlersBuilder(services);

            configurator.Invoke(builder);

            services.TryAddSingleton(provider => builder.BuildRoutes());
            services.TryAddTransient<WebSocketHandlerResolver>();

            return services;
        }
    }
}