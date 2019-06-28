using Android.App;
using Android.Content.PM;
using Android.OS;
using IdentityModel.OidcClient.Browser;
using LibraProgramming.ChatRoom.Client.Droid.Services;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Droid
{
    [Activity(
        Label = "LibraProgramming.ChatRoom.Client", 
        Icon = "@mipmap/ic_launcher", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.Init(this, bundle);
            Xamarin.Essentials.Platform.Init(this, bundle);

            LoadApplication(new App(new AndroidInitializer()));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry registry)
        {
            // Register any platform specific implementations
            registry.Register<IBrowser, ChromeCustomTabBrowser>();
        }
    }
}