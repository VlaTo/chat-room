using System;
using System.IO;
using System.Net;

namespace LibraProgramming.ChatRoom.Client.Services
{
    internal static class WebResponseExtensions
    {
        public static StreamReader GetResponseReader(this WebResponse response)
        {
            if (null == response)
            {
                throw new ArgumentNullException(nameof(response));
            }

            var stream = response.GetResponseStream();

            if (null == stream)
            {
                throw new Exception();
            }

            return new StreamReader(stream);
        }
    }
}