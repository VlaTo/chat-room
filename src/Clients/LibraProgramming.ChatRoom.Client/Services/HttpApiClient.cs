using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;
using Unity;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public class HttpApiClient : IApiClient
    {
        private readonly HttpClient httpClient;

        [InjectionConstructor]
        public HttpApiClient()
            : this(GlobalSettings.Instance.BaseIdentityHostPath)
        {
        }

        public HttpApiClient(Uri basePath)
        {
            httpClient = new HttpClient
            {
                BaseAddress = basePath
            };
        }

        public async Task<T> GetAsync<T>(Uri relativeUri)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, relativeUri))
            {
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    using (var response = await httpClient.SendAsync(request, CancellationToken.None))
                    {
                        var message = response.EnsureSuccessStatusCode();

                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            using (var reader = new JsonTextReader(new StreamReader(stream)))
                            {
                                var serializer = new JsonSerializer();
                                return serializer.Deserialize<T>(reader);
                            }
                        }
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public void SetBearerToken(string bearerToken)
        {
            httpClient.SetBearerToken(bearerToken);
        }
    }
}