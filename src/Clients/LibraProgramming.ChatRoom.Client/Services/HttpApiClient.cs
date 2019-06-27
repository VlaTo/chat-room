using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
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

            var temp = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                // developer hack
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            httpClient = new HttpClient(temp)
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

                    if ((e.HResult & 65535) == 12045)
                    {
                        //var errors = request.TransportInformation.ServerCertificateErrors;
                    }

                    return default;
                }
            }
        }

        public void SetBearerToken(string bearerToken)
        {
            httpClient.SetBearerToken(bearerToken);
        }

        private X509Certificate2 LoadCertificate(string thumbprint)
        {
            using (var store = new X509Store(StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                if (0 < certificates.Count)
                {
                    return certificates[0];
                }
            }

            return null;
        }
    }
}