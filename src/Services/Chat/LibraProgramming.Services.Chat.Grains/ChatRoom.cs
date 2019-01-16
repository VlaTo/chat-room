using System;
using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts;
using LibraProgramming.Services.Chat.Contracts.Models;
using Orleans;
using Orleans.Providers;

namespace LibraProgramming.Services.Chat.Grains
{
    public sealed class ChatRoomDescription
    {
        public string Name
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Guid StreamId
        {
            get;
            set;
        }

        public DateTime Updated
        {
            get;
            set;
        }
    }

    public sealed class ChatRoomState
    {
        public ChatRoomDescription Description
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [StorageProvider(ProviderName = Constants.StorageProviders.ChatRooms)]
    public class ChatRoom : Grain<ChatRoomState>, IChatRoom
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<RoomDescription> GetDescriptionAsync()
        {
            return Task.FromResult(new RoomDescription
            {
                Description = State.Description.Description
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Task SetDescriptionAsync(string description)
        {
            State.Description = new ChatRoomDescription
            {
                Description = description,
                Updated = DateTime.UtcNow
            };

            return WriteStateAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<Guid> JoinAsync()
        {
            if (Guid.Empty == State.Description.StreamId)
            {
                State.Description.StreamId = Guid.NewGuid();

                await WriteStateAsync();
            }

            return State.Description.StreamId;
        }

        public override Task OnActivateAsync()
        {
            if (null == State.Description)
            {
                State.Description = new ChatRoomDescription
                {
                    Name = String.Empty,
                    Description = String.Empty,
                    StreamId = Guid.Empty,
                    Updated = DateTime.UtcNow
                };
            }

            return base.OnActivateAsync();
        }
    }
}
