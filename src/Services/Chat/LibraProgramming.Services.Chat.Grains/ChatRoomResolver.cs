using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;

namespace LibraProgramming.Services.Chat.Grains
{
    public sealed class ChatRoomResolverState
    {
        public Dictionary<long, string> Rooms
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [StorageProvider(ProviderName = Constants.StorageProviders.ChatRooms)]
    public class ChatRoomResolver : Grain<ChatRoomResolverState>, IChatRoomResolver
    {
        private IAsyncStream<RoomActionMessage> actions;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<IReadOnlyDictionary<long, string>> GetRoomsAsync()
        {
            return Task.FromResult<IReadOnlyDictionary<long, string>>(
                new ReadOnlyDictionary<long, string>(State.Rooms)
            );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<long> RegisterRoomAsync(string name)
        {
            if (State.Rooms.ContainsValue(name))
            {
                throw new DuplicateNameException();
            }

            var idManager = GrainFactory.GetGrain<IRoomIdManager>(RoomIdManager.DefaultId);
            var id = await idManager.GenerateIdAsync();
            //var room = GrainFactory.GetGrain<IChatRoom>(id);

            State.Rooms.Add(id, name);

            await Task.WhenAll(
                WriteStateAsync(),
                actions.OnNextAsync(new RoomActionMessage
                {
                    Action = RoomAction.Registered,
                    RoomId = id
                })
            );

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> RenameRoomAsync(long id, string name)
        {
            if (false == State.Rooms.ContainsKey(id))
            {
                throw new InvalidOperationException();
            }

            State.Rooms[id] = name;

            await Task.WhenAll(
                WriteStateAsync(),
                actions.OnNextAsync(new RoomActionMessage
                {
                    Action = RoomAction.Renamed,
                    RoomId = id
                })
            );

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> RemoveRoomAsync(long id)
        {
            if (false == State.Rooms.Remove(id))
            {
                return false;
            }

            //var room = GrainFactory.GetGrain<IChatRoom>(id);

            await Task.WhenAll(
                WriteStateAsync(),
                actions.OnNextAsync(new RoomActionMessage
                {
                    Action = RoomAction.Removed,
                    RoomId = id
                })
            );

            return true;
        }

        public override Task OnActivateAsync()
        {
            if (null == State.Rooms)
            {
                State.Rooms = new Dictionary<long, string>();
            }

            var provider = GetStreamProvider(Constants.StreamProviders.ChatRooms);

            actions = provider.GetStream<RoomActionMessage>(Constants.Streams.ChatRooms, "Rooms");

            return base.OnActivateAsync();
        }
    }
}