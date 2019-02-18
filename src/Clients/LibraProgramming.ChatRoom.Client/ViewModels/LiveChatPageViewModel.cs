using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using LibraProgramming.ChatRoom.Client.Services;
using Prism.Navigation;
using Prism.Navigation.Xaml;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    //[Con]
    public class LiveChatPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService chatService;
        private string description;
        private string message;
        private IChatChannel channel;

        public ObservableCollection<ChatMessageViewModel> Messages
        {
            get;
        }

        public DelegateCommand SendCommand
        {
            get;
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public LiveChatPageViewModel(
            INavigationService navigationService,
            IChatRoomService chatService)
            : base(navigationService)
        {
            this.chatService = chatService;

            SendCommand = new DelegateCommand(OnSendCommand);
            Messages = new ObservableCollection<ChatMessageViewModel>();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            var id = parameters.GetValue<long>("room");
            var room = await chatService.GetRoomAsync(id, CancellationToken.None);

            if (null == room)
            {
                return;
            }

            Title = room.Title;
            Description = room.Description;

            channel = await chatService.OpenChatAsync(id, CancellationToken.None);
            channel.MessageArrived += OnMessageArrived;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            channel.MessageArrived -= OnMessageArrived;
            channel.Dispose();
        }

        private void OnSendCommand()
        {
            channel.SendAsync(Message);
            Message = String.Empty;
        }

        private void OnMessageArrived(object sender, ChatMessageEventArgs e)
        {
            Messages.Add(new ChatMessageViewModel
            {
                Author = e.Message.Author,
                Text = e.Message.Content,
                Created = e.Message.Created
            });
        }
    }
}