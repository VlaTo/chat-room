using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketHandlersBuilder
    {
        private readonly IServiceCollection services;
        private readonly IDictionary<string, Type> handlers;

        public void Register<TWebSocketHandler>(string path)
            where TWebSocketHandler : WebSocketHandler
        {
            if (null == path)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var handlerType = typeof(TWebSocketHandler);

            if (handlers.TryAdd(path, handlerType))
            {
                services.TryAddTransient(handlerType);
            }
        }

        public WebSocketHandlersBuilder(IServiceCollection services)
        {
            this.services = services;
            handlers = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }

        public WebSocketRouteCollection BuildRoutes()
        {
            var collection = new WebSocketRouteCollection();

            foreach (var kvp in handlers)
            {
                var template = TemplateParser.Parse(kvp.Key);
                collection.Register(template, kvp.Value);
            }

            return collection;
        }
    }
}