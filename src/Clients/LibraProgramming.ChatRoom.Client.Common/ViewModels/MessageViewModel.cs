using System;
using LibraProgramming.ChatRoom.Client.Common.Core;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private string author;
        private string content;
        private DateTime created;

        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }

        public DateTime Created
        {
            get => created;
            set => SetProperty(ref created, value);
        }
    }
}