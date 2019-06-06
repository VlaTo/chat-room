using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands
{
    internal sealed class EditRoomCommand : IRequest
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

        public EditRoomCommand(long id, string name, string description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}