using AutoMapper;
using IdentityServer4.Configuration;
using IdentityServer4.EntityFramework.DbContexts;
using LibraProgramming.ChatRoom.Domain.Models;
using LibraProgramming.ChatRoom.Domain.Results;
using LibraProgramming.ChatRoom.Services.Chat.Api.Configuration.IdentityServer;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Extensions;
using LibraProgramming.ChatRoom.Services.Chat.Persistence;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Models;
using LibraProgramming.ChatRoom.Services.Chat.Persistence.Seeds;
using LibraProgramming.Services.Chat.Contracts;
using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

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
                        .AddDbContext<ChatIdentityDbContext>(options =>
                        {
                            options.UseSqlite(context.Configuration.GetConnectionString("Chat"));
                        })
                        .AddIdentity<Customer, CustomerRole>(options =>
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

                            //options.Events.RaiseErrorEvents = true;
                            //options.Events.RaiseSuccessEvents = true;

                            options.UserInteraction.LoginUrl = "/account/signin";
                            options.UserInteraction.ConsentUrl = "/consent/confirm";
                            options.UserInteraction.ErrorUrl = "/account/error";
                        })
                        // https://github.com/IdentityServer/IdentityServer4/blob/44651bea9b02c992902639b21205f433aad47d03/src/IdentityServer4/src/Configuration/DependencyInjection/BuilderExtensions/Crypto.cs
                        .AddSigninCredentials(context.Configuration.GetSection("IdentityServer:Key"))
                        .AddAspNetIdentity<Customer>()
                        .AddConfigurationStore(options =>
                        {
                            options.ConfigureDbContext = database =>
                            {
                                database.UseSqlite(context.Configuration.GetConnectionString("Identity"));
                            };
                        })
                        .AddOperationalStore(options =>
                        {
                            options.ConfigureDbContext = database =>
                            {
                                database.UseSqlite(context.Configuration.GetConnectionString("Identity"));
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
                            typeof(Program).Assembly
                        );

                    services
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
                            //options.RootDirectory = "/Pages";
                        })
                        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

                    services
                        .AddHsts(options =>
                        {
                            options.Preload = true;
                            options.IncludeSubDomains = true;
                            //options.ExcludedHosts.Add("http://localhost:5000");
                        })
                        .AddHttpsRedirection(options =>
                        {
                            options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                            options.HttpsPort = 5001;
                        });

                    services
                        .AddOidcStateDataFormatterCache()
                        .AddAuthorization(options =>
                            options.AddPolicy(
                                "Customers",
                                policy => policy.RequireClaim("role", "Customer")
                            )
                        )
                        //.AddAuthentication(IdentityConstants.ApplicationScheme)
                        .AddAuthentication(options =>
                        {
                            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        })
                        .AddJwtBearer(options =>
                        {
                            options.Authority = "https://localhost:5001";
                            options.RequireHttpsMetadata = true;

                            options.Audience = "chat.api";

                            options.TokenValidationParameters.RoleClaimType = "role";
                        })
                        //.AddOpenIdConnect()
                        ;

                    services.ConfigureApplicationCookie(options =>
                    {
                        options.Cookie.Name = "Chat.Identity";
                    });

                    services
                        .AddTransient<ISigninService<Customer, long>, EntityFrameworkSigninService>();

                    var seed = context.Configuration.GetValue<string>("seed");

                    if (false == String.IsNullOrEmpty(seed))
                    {
                        services.AddScoped<ConfigurationDbSetup>();
                    }
                })
                .Configure(app =>
                {
                    var provider = app.ApplicationServices;
                    var environment = provider.GetRequiredService<IWebHostEnvironment>();

                    if (environment.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();
                    }
                    else
                    {
                        app.UseExceptionHandler("/Error");
                        app.UseHsts();
                    }

                    app
                        .UseHttpsRedirection()
                        .UseStaticFiles()
                        .UseWebSockets(new WebSocketOptions
                        {
                            KeepAliveInterval = TimeSpan.FromSeconds(30.0d),
                            ReceiveBufferSize = 8096
                        })
                        .UseMiddleware<WebSocketsMiddleware>()
                        .UseAuthentication()
                        .UseIdentityServer()
                        .UseRouting()
                        .UseAuthorization()
                        .UseEndpoints(routes =>
                        {
                            routes.MapControllers();
                        });
                })
                .Build();

            host.MigrateDbContext<ConfigurationDbContext>(async (context, services) =>
            {
                var setup = services.GetService<ConfigurationDbSetup>();

                if (null != setup)
                {
                    await setup.SeedAsync(
                        IdentityServerConfiguration.GetClients(),
                        IdentityServerConfiguration.GetIdentityResources(),
                        IdentityServerConfiguration.GetApiResources()
                    );
                }
            });

            host.Run();
        }
    }
}