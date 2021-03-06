﻿using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using LibraProgramming.ChatRoom.Client.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Net.Http;
using Xamarin.Essentials;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    // https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Mobile/eShopOnContainers/eShopOnContainers.Core/Services/Identity/IdentityService.cs
    // https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/tree/master/XamarinForms/XamarinFormsClient/XamarinFormsClient.UWP
    public class SignInPageViewModel : PageViewModelBase
    {
        private readonly IApiClient apiClient;
        private readonly OidcClientOptions options;

        public DelegateCommand SignIn
        {
            get;
        }

        public SignInPageViewModel(
            INavigationService navigationService,
            IApiClient apiClient,
            IBrowser browser)
            : base(navigationService)
        {
            this.apiClient = apiClient;
            options = new OidcClientOptions
            {
                Authority = GlobalSettings.Instance.BaseIdentityHostPath.ToString(),
                ClientId = GlobalSettings.Instance.ClientId,
                ClientSecret = GlobalSettings.Instance.ClientSecret,
                Scope = "openid profile chat.api offline_access",
                RedirectUri = GlobalSettings.Instance.Callback,
                Browser = browser,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect,
                BackchannelHandler = new HttpClientHandler
                {
                    AllowAutoRedirect = true,
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                }
            };
            
            SignIn = new DelegateCommand(OnSignInCommand);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            SignIn.Execute();
            base.OnNavigatedTo(parameters);
        }

        private async void OnSignInCommand()
        {
            const string accesstokenkey = "AccessToken";
            var token = await SecureStorage.GetAsync(accesstokenkey);

            if (String.IsNullOrEmpty(token))
            {
                var client = new OidcClient(options);
                var result = await client.LoginAsync(new LoginRequest
                {
                    BrowserDisplayMode = DisplayMode.Visible
                });

                if (result.IsError)
                {
                    return;
                }

                token = result.AccessToken;

                //result.RefreshToken
                //result.User

                await SecureStorage.SetAsync(accesstokenkey, token);
                await SecureStorage.SetAsync("RefreshToken", result.RefreshToken);
            }

            apiClient.SetBearerToken(token);

            await NavigationService.NavigateAsync("nav/main");
        }
    }
}