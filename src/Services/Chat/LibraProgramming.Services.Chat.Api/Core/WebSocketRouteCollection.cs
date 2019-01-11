using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing.Template;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketRouteCollection
    {
        private readonly IList<WebSocketRoute> routes;

        public IReadOnlyCollection<WebSocketRoute> Routes => new ReadOnlyCollection<WebSocketRoute>(routes);

        public WebSocketRouteCollection()
        {
            routes = new List<WebSocketRoute>();
        }

        public void Register(RouteTemplate template, Type handlerType)
        {
            routes.Add(new WebSocketRoute(template, handlerType));
        }
    }
}