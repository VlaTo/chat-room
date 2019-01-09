using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Client.Common.Core;
using LibraProgramming.ChatRoom.Client.Common.Models;
using LibraProgramming.ChatRoom.Client.Common.Services;
using LibraProgramming.ChatRoom.Client.Common.Views;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class RoomsViewModel : BaseViewModel, IHasSetup
    {
        private string title;
        private bool busy;

        public IChatRoomService ChatRoomService
        {
            get;
        }

        public ObservableCollection<ChatRoomViewModel> Rooms
        {
            get;
        }

        public Command LoadItemsCommand
        {
            get;
        }

        public bool IsBusy
        {
            get => busy;
            set => SetProperty(ref busy, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public RoomsViewModel(IChatRoomService service)
        {
            ChatRoomService = service;
            Title = "Browse";
            Rooms = new ObservableCollection<ChatRoomViewModel>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewRoomPage, AddNewRoomMessage>(this, "AddNewRoom", DoAddRoom);
        }

        public Task SetupAsync()
        {
            return ExecuteLoadItemsCommand();
        }

        private async void DoAddRoom(NewRoomPage page, AddNewRoomMessage message)
        {
            var room = await ChatRoomService.SaveRoomAsync(message.Title, message.Description, CancellationToken.None);

            if (null != room)
            {
                Rooms.Add(new ChatRoomViewModel(room.Id)
                {
                    Title = room.Title,
                    Description = room.Description
                });
            }
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Rooms.Clear();

                var rooms = await ChatRoomService.GetRoomsAsync(CancellationToken.None);

                foreach (var room in rooms)
                {
                    Rooms.Add(new ChatRoomViewModel(room.Id)
                    {
                        Title = room.Title,
                        Description = room.Description
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}