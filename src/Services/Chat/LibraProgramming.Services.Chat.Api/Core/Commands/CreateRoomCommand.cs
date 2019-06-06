using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands
{
    internal sealed class CreateRoomCommand : IRequest<long>
    {
        public string Name
        {
            get;
        }

        public string Description
        {
            get;
        }

        public CreateRoomCommand(string name, string description = null)
        {
            Name = name;
            Description = description;
        }
    }
}