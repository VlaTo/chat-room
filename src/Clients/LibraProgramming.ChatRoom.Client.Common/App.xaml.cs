using LibraProgramming.ChatRoom.Client.Common.Services;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using LibraProgramming.ChatRoom.Client.Common.Views;
using Ninject;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LibraProgramming.ChatRoom.Client.Common
{
    public partial class App
    {
        public static StandardKernel Kernel
        {
            get;
        }

        public App()
        {
            InitializeComponent();

            Kernel
                .Bind<IChatRoomService>()
                .To<ChatRoomService>()
                .InSingletonScope();

            Kernel
                .Bind<RoomsViewModel>()
                .ToSelf()
                .InTransientScope();

            MainPage = new AppShell();
        }

        static App()
        {
            Kernel = new StandardKernel();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}