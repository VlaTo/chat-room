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
    internal sealed class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, long>
    {
        private readonly IClusterClient client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public CreateRoomCommandHandler(IClusterClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<long> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
            var id = await resolver.RegisterRoomAsync(request.Name);
            var room = client.GetGrain<IChatRoom>(id);

            await room.SetDescriptionAsync(request.Description);

            return id;
        }
    }
}