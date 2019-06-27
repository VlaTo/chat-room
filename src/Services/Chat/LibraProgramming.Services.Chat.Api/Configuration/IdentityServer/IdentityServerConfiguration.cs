using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Configuration.IdentityServer
{
    internal static class IdentityServerConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("chat.identity", "Chat Identity API"),
                new ApiResource("chat.api", "Chat API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // client credentials client
                new Client
                {
                    ClientId = "xamarin.application",
                    ClientName = "Xamarin mobile application",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowOfflineAccess = true,
                    AllowAccessTokensViaBrowser = true,

                    ClientSecrets =
                    {
                        new Secret("xamarin".Sha256())
                    },

                    RedirectUris =
                    {
                        "xamarinformsclient://callback",
                    },
                    PostLogoutRedirectUris =
                    {
                        ""
                    },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "chat.identity",
                        "chat.api"
                    }
                }
            };
        }
    }
}