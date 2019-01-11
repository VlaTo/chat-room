using LibraProgramming.ChatRoom.Client.Common.Services;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using Ninject;

namespace LibraProgramming.ChatRoom.Client.Common
{
    public partial class App
    {
        public static IReadOnlyKernel Kernel
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
            var configuration = new KernelConfiguration();

            configuration
                .Bind<IChatRoomService>()
                .To<ChatRoomService>()
                .InSingletonScope();

            configuration
                .Bind<RoomsViewModel>()
                .ToSelf()
                .InTransientScope();

            Kernel = configuration.BuildReadonlyKernel();
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
