using System;
using AutoMapper;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.ChatRoom.Services.Chat.Api.Models;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Domain;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace LibraProgramming.ChatRoom.Services.Chat.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    var environment = context.HostingEnvironment;

                    config
                        .SetBasePath(environment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables();

                    if (environment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }

                    if (null != args)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((context, services) =>
                {
                    var environment = context.HostingEnvironment;

                    if (environment.IsDevelopment())
                    {

                    }

                    services
                        .TryAddSingleton(provider =>
                        {
                            var client = new ClientBuilder()
                                .UseLocalhostClustering()
                                .Configure<ClusterOptions>(options =>
                                {
                                    options.ClusterId = "dev";
                                    options.ServiceId = "chatroom-local";
                                })
                                .ConfigureLogging(logging =>
                                {
                                    logging.AddConsole();
                                })
                                .AddSimpleMessageStreamProvider(Constants.StreamProviders.ChatRooms)
                                .Build();

                            var awaiter = client.Connect().GetAwaiter();

                            awaiter.GetResult();

                            return client;
                        });

                    services
                        .AddAutoMapper(options =>
                        {
                            options
                                .CreateMap<RoomCreateResponse, RoomOperationResult>()
                                .ForMember(
                                    result => result.Id,
                                    map => map.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    result => result.Name,
                                    map => map.MapFrom(source => source.Name)
                                )
                                .ForMember(
                                    result => result.Description,
                                    map => map.MapFrom(source => source.Description)
                                );

                            options
                                .CreateMap<RoomEditResponse, RoomOperationResult>()
                                .ForMember(
                                    result => result.Id,
                                    map => map.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    result => result.Name,
                                    map => map.MapFrom(source => source.Name)
                                )
                                .ForMember(
                                    result => result.Description,
                                    map => map.MapFrom(source => source.Description)
                                );

                            options
                                .CreateMap<RoomResolveResponse, RoomOperationResult>()
                                .ForMember(
                                    result => result.Id,
                                    map => map.MapFrom(source => source.Id)
                                )
                                .ForMember(
                                    result => result.Name,
                                    map => map.MapFrom(source => source.Name)
                                )
                                .ForMember(
                                    result => result.Description,
                                    map => map.MapFrom(source => source.Description)
                                );
                        });

                    services
                        .AddWebSocketHandlers(options =>
                        {
                            options.Register<ChatRoomWebSocketHandler>("api/chat/{room:long}");
                        });

                    services
                        .AddMvcCore()
                        .AddJsonFormatters(options =>
                        {
                            options.ContractResolver = new DefaultContractResolver();
                        });
                })
                .Configure(app =>
                {
                    var provider = app.ApplicationServices;
                    var environment = provider.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        var logger = provider.GetRequiredService<ILogger<Program>>();

                        logger.LogDebug("Application run in development mode");

                        app.UseDeveloperExceptionPage();
                    }

                    app
                        .UseWebSockets(new WebSocketOptions
                        {
                            KeepAliveInterval = TimeSpan.FromSeconds(30.0d),
                            ReceiveBufferSize = 8096
                        })
                        .UseMiddleware<WebSocketsMiddleware>()
                        .UseMvc();
                })
                .Build();

            Console.Title = "Chat Api";

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

            }

            host.Run();
        }
    }
}
