﻿using LibraProgramming.ChatRoom.Client.Services;
using LibraProgramming.ChatRoom.Client.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using NavigationParameters = Prism.Navigation.NavigationParameters;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class MainPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService roomService;
        private bool isBusy;
        private bool isEmpty;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ChatRoomViewModel> Rooms
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand RefreshRoomsCommand
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand AddRoomCommand
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public DelegateCommand<ChatRoomViewModel> NavigateCommand
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get => isEmpty;
            set => SetProperty(ref isEmpty, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="navigationService"></param>
        /// <param name="roomService"></param>
        public MainPageViewModel(
            INavigationService navigationService, 
            IChatRoomService roomService)
            : base(navigationService)
        {
            if (null == roomService)
            {
                throw new ArgumentNullException(nameof(roomService));
            }

            this.roomService = roomService;

            Title = "Chats";
            Rooms = new ObservableCollection<ChatRoomViewModel>();
            RefreshRoomsCommand = new DelegateCommand(OnRefreshRoomsCommand);
            AddRoomCommand = new DelegateCommand(OnAddRoomCommand);
            NavigateCommand = new DelegateCommand<ChatRoomViewModel>(OnNavigateCommand);
            IsEmpty = true;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            RefreshRoomsCommand.Execute();
        }

        private async void OnAddRoomCommand()
        {
            var result = await NavigationService.NavigateAsync(
                $"{nameof(AddNewRoomPage)}",
                parameters: new NavigationParameters
                {
                    {"link", true}
                },
                useModalNavigation: true,
                animated: true
            );

            if (false == result.Success)
            {
                throw result.Exception;
            }
        }

        private async void OnNavigateCommand(ChatRoomViewModel room)
        {
            var result = await NavigationService.NavigateAsync(
                $"{nameof(LiveChatPage)}",
                new NavigationParameters
                {
                    {"room", room.Id}
                },
                false
            );

            if (false == result.Success)
            {
                throw result.Exception;
            }
        }

        private async void OnRefreshRoomsCommand()
        {
            IsBusy = true;

            try
            {
                var rooms = await roomService.GetRoomsAsync();

                Rooms.Clear();

                if (null == rooms)
                {
                    return;
                }

                foreach (var room in rooms)
                {
                    Rooms.Add(new ChatRoomViewModel(room.Id)
                    {
                        Title = room.Title,
                        Description = room.Description
                    });
                }
            }
            finally
            {
                IsEmpty = 0 == Rooms.Count;
                IsBusy = false;
            }
        }
    }
}
