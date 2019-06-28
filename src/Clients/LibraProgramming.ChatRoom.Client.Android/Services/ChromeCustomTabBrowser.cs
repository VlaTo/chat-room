using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Support.CustomTabs;
using IdentityModel.OidcClient.Browser;
using Plugin.CurrentActivity;
using Unity;

namespace LibraProgramming.ChatRoom.Client.Droid.Services
{
    public class ChromeCustomTabBrowser : IBrowser
    {
        private readonly Activity context;
        private readonly CustomTabsActivityManager manager;

        [InjectionConstructor]
        public ChromeCustomTabBrowser()
            : this(CrossCurrentActivity.Current.Activity)
        {
        }

        public ChromeCustomTabBrowser(Activity context)
        {
            this.context = context;
            manager = new CustomTabsActivityManager(context);
            //manager = CustomTabsActivityManager.From(context);
        }

        public Task<BrowserResult> InvokeAsync(BrowserOptions options)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();
            var builder = new CustomTabsIntent.Builder(manager.Session)
                .SetToolbarColor(Color.Argb(255, 52, 152, 219))
                .SetShowTitle(true)
                .EnableUrlBarHiding();

            var customTabsIntent = builder.Build();

            try
            {
                // ensures the intent is not kept in the history stack, which makes
                // sure navigating away from it will close it
                customTabsIntent.Intent.AddFlags(ActivityFlags.NoHistory);

                void Callback(string url)
                {
                    OidcCallbackActivity.Callbacks -= Callback;

                    tcs.SetResult(new BrowserResult
                    {
                        Response = url
                    });
                }

                OidcCallbackActivity.Callbacks += Callback;

                customTabsIntent.LaunchUrl(context, Android.Net.Uri.Parse(options.StartUrl));
            }
            catch (Exception exception)
            {
                tcs.SetException(exception);
            }

            return tcs.Task;
        }
    }
}