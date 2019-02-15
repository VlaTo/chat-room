using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Models;
using LibraProgramming.ChatRoom.Services.Chat.Api.Core.Queries;
using LibraProgramming.Services.Chat.Contracts;
using MediatR;
using Orleans;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Services.Chat.Api.Core.Handlers
{
    public sealed class GetRoomsListQueryHandler : IRequestHandler<GetRoomsListQuery, IEnumerable<RoomDescription>>
    {
        private readonly IClusterClient client;

        public GetRoomsListQueryHandler(IClusterClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<RoomDescription>> Handle(GetRoomsListQuery request, CancellationToken ct)
        {
            var resolver = client.GetGrain<IChatRoomResolver>(Constants.Resolvers.ChatRooms);
            var rooms = await resolver.GetRoomsAsync();
            var models = new RoomDescription[rooms.Count];
            var index = 0;

            foreach (var (id, name) in rooms)
            {
                var room = client.GetGrain<IChatRoom>(id);
                var description = await room.GetDescriptionAsync();

                models[index++] = new RoomDescription(id, name, description.Description);
            }

            return models;
        }
    }
}
