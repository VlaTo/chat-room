using AutoMapper;
using IdentityServer4.Configuration;
using LibraProgramming.ChatRoom.Domain.Models;
using LibraProgramming.ChatRoom.Domain.Results;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Persistence;
using LibraProgramming.Services.Chat.Persistence.Models;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using Blueshift.EntityFrameworkCore.MongoDB.Infrastructure;

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

                    var connectionString = context.Configuration.GetConnectionString("Identity");

                    services
                        .AddDbContext<ChatIdentityDbContext>(options =>
                        {
                            options.UseMongoDb(connectionString);
                        })
                        .AddIdentity<Customer, IdentityRole>(options =>
                        {
                            options.User.RequireUniqueEmail = true;
                        })
                        .AddEntityFrameworkStores<ChatIdentityDbContext>()
                        .AddDefaultTokenProviders();

                    services
                        .AddIdentityServer(options =>
                        {
                            options.Endpoints = new EndpointsOptions
                            {
                                EnableDiscoveryEndpoint = true,
                                EnableAuthorizeEndpoint = true,
                                EnableCheckSessionEndpoint = true,
                                EnableEndSessionEndpoint = true,
                                EnableUserInfoEndpoint = true
                            };

                            options.Events.RaiseErrorEvents = true;
                            options.Events.RaiseSuccessEvents = true;

                            options.UserInteraction.LoginUrl = "";
                            options.UserInteraction.ConsentUrl = "";
                            options.UserInteraction.ErrorUrl = "";
                        })
                        // https://github.com/IdentityServer/IdentityServer4/blob/44651bea9b02c992902639b21205f433aad47d03/src/IdentityServer4/src/Configuration/DependencyInjection/BuilderExtensions/Crypto.cs
                        //.AddDeveloperSigningCredential()
                        .AddAspNetIdentity<Customer>()
                        .AddConfigurationStore(options =>
                        {
                            options.ConfigureDbContext = database =>
                            {
                                database.UseMongoDb(connectionString);
                            };
                        })
                        .AddOperationalStore(options =>
                        {
                            options.ConfigureDbContext = database =>
                            {
                                database.UseMongoDb(connectionString);
                            };
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

                                options
                                    .CreateMap<RoomDescription, RoomDetails>()
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
                        .AddControllers(options =>
                        {
                            options.EnableEndpointRouting = true;
                        });

                    services
                        .AddRazorPages(options =>
                        {
                            options.RootDirectory = "/Pages";
                        })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                    services
                        .AddOidcStateDataFormatterCache()
                        .AddAuthentication(IdentityConstants.ApplicationScheme)
                        //.AddOpenIdConnect()
                        ;

                    services.ConfigureApplicationCookie(options =>
                    {
                        options.Cookie.Name = "Chat.Identity";
                    });
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