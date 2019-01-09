namespace LibraProgramming.ChatRoom.Client.Common.Models
{
    public class AddNewRoomMessage
    {
        public string Title
        {
            get;
        }

        public string Description
        {
            get;
        }

        public AddNewRoomMessage(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}