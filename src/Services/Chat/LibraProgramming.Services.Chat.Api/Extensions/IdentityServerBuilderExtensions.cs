using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Extensions
{
    internal static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddSigninCredentials(this IIdentityServerBuilder builder, IConfiguration configuration)
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var keyId = configuration[nameof(RsaSecurityKey.KeyId)];

            return builder.AddSigningCredential(CreateRsaSecurityKey(configuration, keyId));
        }

        private static RsaSecurityKey CreateRsaSecurityKey(IConfiguration configuration, string id)
        {
            var section = configuration.GetSection(nameof(RsaSecurityKey.Parameters));
            var keyId = configuration[nameof(RsaSecurityKey.KeyId)];
            var rsaParameters = new RSAParameters
            {
                D = Convert.FromBase64String(section[nameof(RSAParameters.D)]),
                DP = Convert.FromBase64String(section[nameof(RSAParameters.DP)]),
                DQ = Convert.FromBase64String(section[nameof(RSAParameters.DQ)]),
                Exponent = Convert.FromBase64String(section[nameof(RSAParameters.Exponent)]),
                InverseQ = Convert.FromBase64String(section[nameof(RSAParameters.InverseQ)]),
                Modulus = Convert.FromBase64String(section[nameof(RSAParameters.Modulus)]),
                P = Convert.FromBase64String(section[nameof(RSAParameters.P)]),
                Q = Convert.FromBase64String(section[nameof(RSAParameters.Q)])
            };
            
            return new RsaSecurityKey(rsaParameters)
            {
                KeyId = keyId
            };
        }
    }
}