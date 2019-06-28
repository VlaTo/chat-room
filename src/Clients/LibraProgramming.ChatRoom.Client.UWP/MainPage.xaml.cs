using IdentityModel.OidcClient.Browser;
using LibraProgramming.ChatRoom.Client.Services;
using LibraProgramming.ChatRoom.Client.UWP.Extensions;
using LibraProgramming.ChatRoom.Client.UWP.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Prism;
using Prism.Ioc;
using Prism.Navigation;
using Prism.Unity;
using Unity;
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
        public void RegisterTypes(IContainerRegistry registry)
        {
            // Register any platform specific implementations
            var unity = registry.GetContainer();

            unity.AddExtension(new LoggingExtension());

            var factory = new LoggerFactory();

            factory.AddProvider(new DebugLoggerProvider());

            registry.RegisterInstance(typeof(ILogger), factory.CreateLogger("Debug"));
            registry.RegisterSingleton<IUserInformation, UwpUserInformationService>();
            registry.Register<IBrowser, WebAuthBrokerBrowser>();
        }
    }
}
