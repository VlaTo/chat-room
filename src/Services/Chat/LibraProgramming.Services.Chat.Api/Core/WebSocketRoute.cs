using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core
{
    public class WebSocketRoute
    {
        public RouteTemplate Template
        {
            get;
        }

        public Type HandlerType
        {
            get;
        }

        public WebSocketRoute(RouteTemplate template, Type handlerType)
        {
            Template = template;
            HandlerType = handlerType;
        }
    }
}