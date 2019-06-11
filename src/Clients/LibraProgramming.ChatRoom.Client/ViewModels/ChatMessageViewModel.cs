using System;
using Prism.Mvvm;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public sealed class ChatMessageViewModel : BindableBase
    {
        private string author;
        private string text;
        private bool isMyMessage;
        private DateTime created;

        public bool IsMyMessage
        {
            get => isMyMessage;
            set => SetProperty(ref isMyMessage, value);
        }

        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public DateTime Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
    }
}