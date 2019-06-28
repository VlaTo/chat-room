using LibraProgramming.ChatRoom.Client.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using LibraProgramming.ChatRoom.Client.Controls;
using LibraProgramming.ChatRoom.Client.Models.Data;
using LibraProgramming.ChatRoom.Client.Persistence;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class LiveChatPageViewModel : PageViewModelBase
    {
        private readonly IChatRoomService chatService;
        private readonly IMessageService messageService;
        private readonly InteractionRequest<NewMessageContext> newMessageRequest;
        private readonly ChatDbContext context;
        private string description;
        private string text;
        private IChatChannel channel;
        private IPrincipal author;
        private long roomId;

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
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public IInteractionRequest NewMessageRequest => newMessageRequest;

        public LiveChatPageViewModel(
            INavigationService navigationService,
            IChatRoomService chatService,
            //IUserInformation userInformation,
            ChatDbContext context)
            : base(navigationService)
        {
            this.chatService = chatService;
            //this.userInformation = userInformation;
            this.context = context;

            SendCommand = new DelegateCommand(OnSendCommand);
            Messages = new ObservableCollection<ChatMessageViewModel>();
            newMessageRequest = new InteractionRequest<NewMessageContext>();

            /*Messages.Add(new ChatMessageViewModel
            {
                Author = "User0123",
                IsMyMessage = false,
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut sagittis urna nisi, eget efficitur ipsum posuere sed.",
                Created = DateTime.Now - TimeSpan.FromHours(1.0d)
            });
            Messages.Add(new ChatMessageViewModel
            {
                Author = "User0123",
                IsMyMessage = false,
                Text = "Nullam tristique urna non tortor iaculis",
                Created = DateTime.Now - TimeSpan.FromHours(1.5d)
            });*/
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            roomId = parameters.GetValue<long>("room");
            var room = await context.Rooms.FirstOrDefaultAsync(entity => entity.Id == roomId);

            if (null == room)
            {
                var chatRoom = await chatService.GetRoomAsync(roomId);
                room = new Room
                {
                    Title = chatRoom.Title,
                    Description = chatRoom.Description,
                    Messages = new Collection<Message>()
                };

                await context.Rooms.AddAsync(room);
                await context.SaveChangesAsync();
            }

            //var username = await userInformation.GetUserNameAsync();
            author = new GenericPrincipal(
                new GenericIdentity("TestUser", ClaimsIdentity.DefaultNameClaimType),
                new[] {"Author"}
            );

            var messages = await context.Messages
                .Where(entity => entity.RoomId == roomId)
                .OrderBy(entity => entity.Id)
                .ToArrayAsync();

            foreach (var item in messages)
            {
                Messages.Add(new ChatMessageViewModel
                {
                    Author = item.Author,
                    Text = item.Text,
                    Created = item.Created
                });
            }

            channel = await chatService.OpenChatAsync(roomId, author, CancellationToken.None);
            channel.MessageArrived += OnMessageArrived;

            foreach (var message in messageService.GetMessages())
            {
                Messages.Add(new ChatMessageViewModel
                {
                    Author = message.Author,
                    IsMyMessage = IsSameAuthor(message.Author),
                    Text = message.Content,
                    Created = message.Created
                });
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            channel.MessageArrived -= OnMessageArrived;
            channel.Dispose();
        }

        private async void OnSendCommand()
        {
            var message = Message;
            Message = String.Empty;
            await channel.SendAsync(message);
        }

        private async void OnMessageArrived(object sender, ChatMessageEventArgs e)
        {
            var message = e.Message;

            context.Messages.Add(new Message
            {
                Author = message.Author,
                Text = message.Content,
                Created = message.Created,
                RoomId = roomId
            });

            await context.SaveChangesAsync();

            var model = new ChatMessageViewModel
            {
                Author = message.Author,
                IsMyMessage = IsSameAuthor(message.Author),
                Text = message.Content,
                Created = message.Created
            };

            newMessageRequest.Raise(
                new NewMessageContext(() => Messages.Add(model)),
                () => { }
            );
        }

        private bool IsSameAuthor(string name)
        {
            var currentName = author.Identity.Name;
            return String.Equals(currentName, name, StringComparison.OrdinalIgnoreCase);
        }
    }
}