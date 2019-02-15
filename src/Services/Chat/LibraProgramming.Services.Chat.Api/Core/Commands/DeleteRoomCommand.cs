using MediatR;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands
{
    public sealed class DeleteRoomCommand : IRequest
    {
        public long Id
        {
            get;
        }

        public DeleteRoomCommand(long id)
        {
            Id = id;
        }
    }
}