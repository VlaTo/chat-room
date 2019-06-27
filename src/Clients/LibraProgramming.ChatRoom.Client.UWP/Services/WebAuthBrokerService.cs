using System;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Unity;

namespace LibraProgramming.ChatRoom.Client.UWP.Services
{
    public sealed class WebAuthBrokerService : IBrowser
    {
        private readonly bool enableWindowsAuthentication;

        [InjectionConstructor]
        public WebAuthBrokerService()
            : this(false)
        {
        }

        public WebAuthBrokerService(bool enableWindowsAuthentication)
        {
            this.enableWindowsAuthentication = enableWindowsAuthentication;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var wabOptions = GetWebAuthOptions(options, false);
            WebAuthenticationResult result;

            try
            {
                var brokenAbsolutePath = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;
                var flag = String.Equals(options.EndUrl, brokenAbsolutePath, StringComparison.Ordinal);
                var requestUri = new Uri(options.StartUrl);

                result = flag
                    ? await WebAuthenticationBroker.AuthenticateAsync(wabOptions, requestUri)
                    : await WebAuthenticationBroker.AuthenticateAsync(wabOptions, requestUri, new Uri(options.EndUrl));
            }
            catch (Exception ex)
            {
                return new BrowserResult
                {
                    ResultType = BrowserResultType.UnknownError,
                    Error = ex.ToString()
                };
            }

            switch (result.ResponseStatus)
            {
                case WebAuthenticationStatus.Success:
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.Success,
                        Response = result.ResponseData
                    };
                }

                case WebAuthenticationStatus.ErrorHttp:
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.HttpError,
                        Error = result.ResponseErrorDetail.ToString()
                    };
                }

                case WebAuthenticationStatus.UserCancel:
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.UserCancel
                    };
                }

                default:
                {
                    return new BrowserResult
                    {
                        ResultType = BrowserResultType.UnknownError,
                        Error = "Invalid response from WebAuthenticationBroker"
                    };
                }
            }
        }

        private WebAuthenticationOptions GetWebAuthOptions(BrowserOptions options, bool silentMode)
        {
            var result = WebAuthenticationOptions.None;

            if (options.ResponseMode == OidcClientOptions.AuthorizeResponseMode.FormPost)
            {
                result |= WebAuthenticationOptions.UseHttpPost;
            }

            if (enableWindowsAuthentication)
            {
                result |= WebAuthenticationOptions.UseCorporateNetwork;
            }

            if (silentMode)
            {
                result |= WebAuthenticationOptions.SilentMode;
            }

            return result;
        }
    }
}