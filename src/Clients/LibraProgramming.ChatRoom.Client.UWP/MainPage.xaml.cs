using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.UWP
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MainPage : IMasterDetailPageOptions
    {
        public bool IsPresentedAfterNavigation => Device.Idiom != TargetIdiom.Phone;

        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new Client.App(new UwpInitializer()));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UwpInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}
