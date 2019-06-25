using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public sealed class AuthorizeRequest
    {
        readonly Uri authorizeEndpoint;

        public AuthorizeRequest(string authorizeEndpoint)
        {
            this.authorizeEndpoint = new Uri(authorizeEndpoint);
        }

        public string Create(IDictionary<string, string> values)
        {
            var queryString = String.Join("&", values
                .Select(kvp => String.Format("{0}={1}", WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value)))
                .ToArray()
            );
            return String.Format("{0}?{1}", authorizeEndpoint.AbsoluteUri, queryString);
        }
    }
}