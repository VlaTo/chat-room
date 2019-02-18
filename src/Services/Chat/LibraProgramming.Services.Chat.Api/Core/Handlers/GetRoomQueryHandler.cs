using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries;
using LibraProgramming.Services.Chat.Contracts;
using MediatR;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRoomQueryHandler : IRequestHandler<GetRoomQuery, RoomDescription>
    {
        private readonly IClusterClient client;

        public GetRoomQueryHandler(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<RoomDescription> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
            var rooms = await resolver.GetRoomsAsync();

            if (false == rooms.TryGetValue(request.Id, out var name))
            {
                return null;
            }

            var room = client.GetGrain<IChatRoom>(request.Id);
            var info = await room.GetDescriptionAsync();

            return new RoomDescription(request.Id, name, info.Description);
        }
    }
}