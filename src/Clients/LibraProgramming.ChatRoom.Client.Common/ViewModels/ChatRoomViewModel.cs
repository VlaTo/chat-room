using LibraProgramming.ChatRoom.Client.Common.Core;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class ChatRoomViewModel : BaseViewModel
    {
        private string title;
        private string description;

        public long Id
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

        public ChatRoomViewModel(long id)
        {
            Id = id;
        }
    }
}