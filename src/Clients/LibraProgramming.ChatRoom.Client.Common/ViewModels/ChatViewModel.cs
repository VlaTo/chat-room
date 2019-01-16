using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Client.Common.Core;
using LibraProgramming.ChatRoom.Client.Common.Services;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class ChatViewModel : BaseViewModel, IHasSetup, IHasCleanup
    {
        private readonly IChatRoomService service;
        private string title;
        private string description;
        private string text;
        private IChatChannel channel;

        public ObservableCollection<MessageViewModel> Messages
        {
            get;
        }

        public long RoomId
        {
            get;
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public Command Send
        {
            get;
        }

        public ChatViewModel(IChatRoomService service, ChatRoomViewModel room)
        {
            this.service = service;
            Messages = new ObservableCollection<MessageViewModel>();
            Send = new Command(DoSend);
            RoomId = room.Id;
            Title = room.Title;
            Description = room.Description;
        }

        public async Task SetupAsync()
        {
            channel = await service.OpenChatAsync(RoomId, CancellationToken.None);
            channel.MessageArrived += OnMessageArrived;
        }

        public Task CleanupAsync()
        {
            channel.MessageArrived -= OnMessageArrived;
            channel?.Dispose();

            return Task.CompletedTask;
        }

        private void OnMessageArrived(object sender, ChatMessageEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Messages.Add(new MessageViewModel
                {
                    Author = e.Message.Author,
                    Content = e.Message.Content,
                    Created = e.Message.Created
                });
            });
        }

        private async void DoSend()
        {
            var content = Text;

            Text = String.Empty;
            
           await channel.SendAsync(content);
        }
    }
}
