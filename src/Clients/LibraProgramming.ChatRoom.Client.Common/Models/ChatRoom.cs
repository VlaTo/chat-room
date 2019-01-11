namespace LibraProgramming.ChatRoom.Client.Common.Models
{
    public class ChatRoom : IEntity<long>
    {
        public long Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}