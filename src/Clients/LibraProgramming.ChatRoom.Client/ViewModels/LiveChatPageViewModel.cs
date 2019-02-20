using LibraProgramming.ChatRoom.Client.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class LiveChatPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService chatService;
        private string description;
        private string message;
        private IChatChannel channel;
        private IPrincipal author;

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

            Messages.Add(new ChatMessageViewModel
            {
                Author = "User0123",
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut sagittis urna nisi, eget efficitur ipsum posuere sed.",
                Created = DateTime.Now - TimeSpan.FromHours(1.0d)
            });
            Messages.Add(new ChatMessageViewModel
            {
                Author = "User0123",
                Text = "Nullam tristique urna non tortor iaculis",
                Created = DateTime.Now - TimeSpan.FromHours(1.5d)
            });
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

            author = new GenericPrincipal(
                new GenericIdentity($"User{(ushort) DateTime.Now.Ticks}", ClaimsIdentity.DefaultNameClaimType),
                new[] {"Author"}
            );

            channel = await chatService.OpenChatAsync(id, author, CancellationToken.None);
            channel.MessageArrived += OnMessageArrived;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            channel.MessageArrived -= OnMessageArrived;
            channel.Dispose();
        }

        private void OnSendCommand()
        {
            var message = Message;
            Message = String.Empty;
            channel.SendAsync(message);
        }

        private void OnMessageArrived(object sender, ChatMessageEventArgs e)
        {
            //Debug.WriteLine($"[LiveChatPageViewModel.OnMessageArrived] {e.Message.Content}");

            Messages.Add(new ChatMessageViewModel
            {
                Author = e.Message.Author,
                Text = e.Message.Content,
                Created = e.Message.Created
            });
        }
    }
}