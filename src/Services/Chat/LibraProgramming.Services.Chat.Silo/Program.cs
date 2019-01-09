using System;
using System.Net;
using LibraProgramming.Services.Chat.Contracts;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LibraProgramming.Services.Chat.Silo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var siloBuilder = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "chatroom-local";
                })
                .Configure<EndpointOptions>(options =>
                {
                    options.AdvertisedIPAddress = IPAddress.Loopback;
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .AddMemoryGrainStorage("PubSubStore")
                .AddMongoDBGrainStorage(Constants.StorageProviders.ChatRooms, options =>
                {
                    options.ConnectionString = "mongodb://localhost:27017/chatroom";
                })
                .AddSimpleMessageStreamProvider(Constants.StreamProviders.ChatRooms);

            Console.Title = "Chat Silo";

            using (var host = siloBuilder.Build())
            {
                var awaiter = host.StartAsync().GetAwaiter();

                awaiter.GetResult();

                Console.Read();
            }
        }
    }
}
