using LibraProgramming.ChatRoom.Client.Common.Core;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private string content;

        public string Content
        {
            get => content;
            set => SetProperty(ref content, value);
        }
    }
}