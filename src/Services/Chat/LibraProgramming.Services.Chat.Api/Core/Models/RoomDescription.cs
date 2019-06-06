namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models
{
    internal sealed class RoomDescription
    {
        public long Id
        {
            get;
        }

        public string Name
        {
            get;
        }

        public string Description
        {
            get;
        }

        public RoomDescription(long id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}