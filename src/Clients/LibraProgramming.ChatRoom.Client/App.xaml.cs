﻿using LibraProgramming.ChatRoom.Client.Services;
using LibraProgramming.ChatRoom.Client.ViewModels;
using LibraProgramming.ChatRoom.Client.Views;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LibraProgramming.ChatRoom.Client
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App()
            : this(null)
        {
        }

        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(Views.MainPage)}");

            if (false == result.Success)
            {
                throw result.Exception;
            }
        }

        protected override void RegisterTypes(IContainerRegistry container)
        {
            container.Register<IChatRoomService, ChatRoomService>();
            //container.RegisterForNavigation<PrismNavigationPage>(nameof(PrismNavigationPage));
            container.RegisterForNavigation<NavigationPage>(nameof(NavigationPage));
            container.RegisterForNavigation<MainPage, MainPageViewModel>(nameof(Views.MainPage));
            container.RegisterForNavigation<LiveChatPage, LiveChatPageViewModel>(nameof(LiveChatPage));
        }
    }
}