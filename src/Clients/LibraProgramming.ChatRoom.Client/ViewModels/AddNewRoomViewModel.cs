using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Client.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public sealed class AddNewRoomViewModel : BindableBase
    {
        private readonly IChatRoomService chatRoomService;
        private readonly INavigationService navigationService;
        private string roomName;
        private string description;

        public string RoomName
        {
            get => roomName;
            set => SetProperty(ref roomName, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public DelegateCommand CancelCommand
        {
            get;
        }

        public DelegateCommand AddRoomCommand
        {
            get;
        }

        public AddNewRoomViewModel(
            IChatRoomService chatRoomService,
            INavigationService navigationService)
        {
            this.chatRoomService = chatRoomService;
            this.navigationService = navigationService;

            AddRoomCommand = new DelegateCommand(OnAddRoomCommand);
            CancelCommand = new DelegateCommand(OnCancelCommand);
        }

        private async void OnAddRoomCommand()
        {
            var room = await chatRoomService.CreateRoomAsync(roomName, description);

            if (null != room)
            {
                ;
            }

            await CloseAsync(new NavigationParameters
            {
                {"room", room.Id}
            });
        }

        private async void OnCancelCommand()
        {
            await CloseAsync(null);
        }

        private async Task CloseAsync(NavigationParameters parameters)
        {
            await navigationService.GoBackAsync(parameters, useModalNavigation: true);
        }
    }
}