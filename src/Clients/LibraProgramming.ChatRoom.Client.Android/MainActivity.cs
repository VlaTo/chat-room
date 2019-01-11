using Android.App;
using Android.Content.PM;
using Android.OS;
using LibraProgramming.ChatRoom.Client.Common;
using Xamarin.Forms.Platform.Android;

namespace LibraProgramming.ChatRoom.Client.Android
{
    [Activity(
        Label = "ChatRoomModel",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation
    )]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags("Shell_Experimental");
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
    }
}