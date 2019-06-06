using AutoMapper;
using LibraProgramming.ChatRoom.Domain.Results;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.Services.Chat.Contracts;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace LibraProgramming.ChatRoom.Services.Chat.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Chat Api";

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
                                // action results
                                options
                                    .CreateMap<RoomDescription, RoomCreatedResult>()
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
                                    .CreateMap<RoomDescription, RoomResult>()
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
                            },
                            AppDomain.CurrentDomain.GetAssemblies())
                        .AddMediatR(
                            typeof(Program).Assembly
                        );

                    services
                        .AddLogging()
                        .AddSingleton<IChatRoomRegistry, ChatRoomRegistry>()
                        .AddWebSocketHandlers(options =>
                        {
                            options.Register<ChatRoomWebSocketHandler>("api/chat/{room:long}");
                        })
                        .AddControllers()
                        /*.AddJsonFormatters(options =>
                        {
                            options.ContractResolver = new DefaultContractResolver();
                        })*/
                        ;
                })
                .Configure(app =>
                {
                    var provider = app.ApplicationServices;
                    var environment = provider.GetRequiredService<IWebHostEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }

                    app
                        .UseWebSockets(new WebSocketOptions
                        {
                            KeepAliveInterval = TimeSpan.FromSeconds(30.0d),
                            ReceiveBufferSize = 8096
                        })
                        .UseMiddleware<WebSocketsMiddleware>()
                        .UseRouting()
                        .UseEndpoints(routes =>
                        {
                            routes.MapControllers();
                        });
                })
                .Build();

            /*using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
            }*/

            host.Run();
        }
    }
}