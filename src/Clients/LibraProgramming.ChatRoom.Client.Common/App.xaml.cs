using LibraProgramming.ChatRoom.Client.Common.Services;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using Ninject;

namespace LibraProgramming.ChatRoom.Client.Common
{
    public partial class App
    {
        public static IKernel Kernel
        {
            get;
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        static App()
        {
            var kernel = new StandardKernel();

            kernel
                .Bind<IChatRoomService>()
                .To<ChatRoomService>()
                .InSingletonScope();

            kernel
                .Bind<RoomsViewModel>()
                .ToSelf()
                .InTransientScope();

            Kernel = kernel;
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
