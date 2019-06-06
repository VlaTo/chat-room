using LibraProgramming.ChatRoom.Client.Services;
using LibraProgramming.ChatRoom.Client.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using LibraProgramming.ChatRoom.Client.Controls;
using Prism.Navigation.Xaml;
using Xamarin.Forms;
using NavigationParameters = Prism.Navigation.NavigationParameters;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class MainPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService roomService;
        private InteractionRequest<AddNewRoomRequestContext> addNewRoomRequest;
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
        public IInteractionRequest AddNewRoomRequest => addNewRoomRequest;

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
            addNewRoomRequest = new InteractionRequest<AddNewRoomRequestContext>();

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
            /*var args = new NavigationParameters
            {
                {"room", obj.Id}
            };*/

            var result = await NavigationService.NavigateAsync(
                $"{nameof(NavigationPage)}/{nameof(AddNewRoomPage)}",
                useModalNavigation: true,
                animated: true
            );

            if (false == result.Success)
            {
                throw result.Exception;
            }
        }

        private async void OnNavigateCommand(ChatRoomViewModel obj)
        {
            var args = new NavigationParameters
            {
                {"room", obj.Id}
            };

            //var navigationUrl = $"{nameof(NavigationPage)}/{nameof(LiveChatPage)}";
            var navigationUrl = $"{nameof(LiveChatPage)}";
            var result = await NavigationService.NavigateAsync(navigationUrl, args, false);

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
