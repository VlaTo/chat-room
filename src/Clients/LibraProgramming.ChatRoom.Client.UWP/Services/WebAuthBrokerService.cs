using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
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

        // https://docs.microsoft.com/en-us/windows/uwp/security/web-authentication-broker
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var wabOptions = GetWebAuthOptions(options, GlobalSettings.Instance.SilentMode);
            WebAuthenticationResult result;

            try
            {
                var brokerAbsolutePath = WebAuthenticationBroker.GetCurrentApplicationCallbackUri().AbsoluteUri;

                result = await BrokerSigninAsync(options, brokerAbsolutePath, wabOptions, CancellationToken.None);
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

        private static Task<WebAuthenticationResult> BrokerSigninAsync(
            BrowserOptions options,
            string brokerAbsolutePath,
            WebAuthenticationOptions wabOptions,
            CancellationToken cancellationToken)
        {
            IAsyncOperation<WebAuthenticationResult> result;
            var requestUri = new Uri(options.StartUrl);

            if (String.Equals(options.EndUrl, brokerAbsolutePath, StringComparison.Ordinal))
            {
                result = WebAuthenticationBroker.AuthenticateAsync(wabOptions, requestUri);
            }
            else
            {
                var callbackUri = new Uri(options.EndUrl);
                result = WebAuthenticationBroker.AuthenticateAsync(wabOptions, requestUri, callbackUri);
            }

            return result.AsTask(cancellationToken);
        }
    }
}