using System;
using System.Collections.Generic;
using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public class IdentityService
    {
        public IdentityService()
        {
        }

        /*public Uri CreateAuthorizationRequest()
        {
            // Create URI to authorization endpoint
            var authorizeRequest = new AuthorizeRequest(GlobalSetting.Instance.AuthorizeEndpoint);

            // Dictionary with values for the authorize request
            var dic = new Dictionary<string, string>
            {
                {"client_id", GlobalSetting.Instance.ClientId},
                {"client_secret", GlobalSetting.Instance.ClientSecret},
                {"response_type", "code id_token"},
                {"scope", "openid profile basket orders locations marketing offline_access"},
                {"redirect_uri", GlobalSetting.Instance.Callback},
                {"nonce", Guid.NewGuid().ToString("N")},
                {"code_challenge", CreateCodeChallenge()},
                {"code_challenge_method", "S256"}
            };

            // Add CSRF token to protect against cross-site request forgery attacks.
            var currentCSRFToken = Guid.NewGuid().ToString("N");
            dic.Add("state", currentCSRFToken);

            return new Uri(authorizeRequest.Create(dic));
        }*/
    }
}