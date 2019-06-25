using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using LibraProgramming.ChatRoom.Client.Services;
using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    // https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Mobile/eShopOnContainers/eShopOnContainers.Core/Services/Identity/IdentityService.cs
    // https://github.com/IdentityModel/IdentityModel.OidcClient.Samples/tree/master/XamarinForms/XamarinFormsClient/XamarinFormsClient.UWP
    public class SignInPageViewModel : PageViewModelBase
    {
        private readonly IApiClient apiClient;
        private readonly OidcClientOptions options;
        private readonly OidcClient client;

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
                Scope = "openid profile email api offline_access",
                RedirectUri = GlobalSettings.Instance.Callback,
                //TokenClientCredentialStyle = ClientCredentialStyle.PostBody,
                Browser = browser,
                ResponseMode = OidcClientOptions.AuthorizeResponseMode.Redirect
            };
            client = new OidcClient(options);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var result = await client.LoginAsync(new LoginRequest
            {
                BrowserDisplayMode = DisplayMode.Visible
            });

            if (result.IsError)
            {
                return;
            }

            //result.RefreshToken
            //result.User

            apiClient.SetBearerToken(result.AccessToken);

            base.OnNavigatedTo(parameters);
        }
    }
}