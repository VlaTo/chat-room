using Prism.Mvvm;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChatRoomViewModel : BindableBase
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