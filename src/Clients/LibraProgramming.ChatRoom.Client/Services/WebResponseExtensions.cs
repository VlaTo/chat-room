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

        public static HttpWebResponse EnsureSuccessResult(this WebResponse response)
        {
            if (null == response)
            {
                throw new ArgumentNullException(nameof(response));
            }

            if (response is HttpWebResponse httpResponse)
            {
                if (false == IsSuccess(httpResponse.StatusCode))
                {
                    throw new Exception();
                }

                return httpResponse;
            }

            throw new NotSupportedException();
        }

        private static bool IsSuccess(HttpStatusCode code)
        {
            return HttpStatusCode.OK == code || HttpStatusCode.Accepted == code || HttpStatusCode.Created == code;
        }
    }
}