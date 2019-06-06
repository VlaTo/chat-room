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
    // ReSharper disable once UnusedMember.Global
    internal sealed class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand>
    {
        private readonly IClusterClient client;

        public DeleteRoomCommandHandler(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<Unit> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);

            if (false == await resolver.RemoveRoomAsync(request.Id))
            {
                throw new InvalidOperationException();
            }

            return Unit.Value;
        }
    }
}