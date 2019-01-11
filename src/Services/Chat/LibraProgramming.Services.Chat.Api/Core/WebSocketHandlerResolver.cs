using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketHandlerResolver
    {
        private readonly WebSocketRouteCollection _routeCollection;
        private readonly IServiceProvider provider;

        public WebSocketHandlerResolver(WebSocketRouteCollection routeCollection, IServiceProvider provider)
        {
            this._routeCollection = routeCollection;
            this.provider = provider;
        }

        public WebSocketHandler ResolveHandler(PathString path, RouteValueDictionary values)
        {
            foreach (var route in _routeCollection.Routes)
            {
                var matcher = new TemplateMatcher(route.Template, new RouteValueDictionary());

                if (matcher.TryMatch(path, values))
                {
                    return (WebSocketHandler)provider.GetService(route.HandlerType);
                }
            }

            return null;
        }
    }
}