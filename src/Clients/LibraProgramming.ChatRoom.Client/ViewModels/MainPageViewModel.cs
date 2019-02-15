using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Client.Services;
using Prism.Commands;
using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class MainPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService roomService;
        private bool isBusy;

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
        public DelegateCommand LoadRoomsCommand
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

            Title = "Main Page";
            Rooms = new ObservableCollection<ChatRoomViewModel>();
            LoadRoomsCommand = new DelegateCommand(OnLoadRoomsCommand);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            LoadRoomsCommand.Execute();
        }

        private async void OnLoadRoomsCommand()
        {
            IsBusy = true;

            try
            {
                var rooms = await roomService.GetRoomsAsync(CancellationToken.None);

                Rooms.Clear();

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
                IsBusy = false;
            }
        }
    }
}
