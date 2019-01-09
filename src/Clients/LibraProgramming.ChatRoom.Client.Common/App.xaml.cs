using LibraProgramming.ChatRoom.Client.Common.Services;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using LibraProgramming.ChatRoom.Client.Common.Views;
using Ninject;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
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

        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        public static string AzureBackendUrl = "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App()
        {
            InitializeComponent();

            //DependencyResolver.ResolveUsing(type => Kernel.Bind(type).ToSelf());

            if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<AzureDataStore>();

            Kernel
                .Bind<IChatRoomService>()
                .To<ChatRoomService>()
                .InSingletonScope();

            Kernel
                .Bind<RoomsViewModel>()
                .ToSelf()
                .InTransientScope();

            //DependencyService.Register<ChatRoomService>();
            //DependencyService.Register<RoomsViewModel>();

            MainPage = new MainPage();
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