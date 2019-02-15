using System;
using System.Threading;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Commands;
using LibraProgramming.Services.Chat.Contracts;
using MediatR;
using Orleans;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EditRoomCommandHandler : IRequestHandler<EditRoomCommand>
    {
        private readonly IClusterClient client;

        public EditRoomCommandHandler(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<Unit> Handle(EditRoomCommand request, CancellationToken cancellationToken)
        {
            var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
            var rooms = await resolver.GetRoomsAsync();

            if (false == rooms.ContainsKey(request.Id))
            {
                throw new InvalidOperationException();
            }

            var room = client.GetGrain<IChatRoom>(request.Id);

            await Task.WhenAll(
                resolver.RenameRoomAsync(request.Id, request.Name),
                room.SetDescriptionAsync(request.Description)
            );

            return Unit.Value;
        }
    }
}