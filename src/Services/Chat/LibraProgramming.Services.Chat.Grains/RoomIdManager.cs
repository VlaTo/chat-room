using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts;
using Orleans;
using Orleans.Providers;

namespace LibraProgramming.Services.Chat.Grains
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LongIdManagerState
    {
        public long? Last
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [StorageProvider(ProviderName = Constants.StorageProviders.ChatRooms)]
    public class RoomIdManager : Grain<LongIdManagerState>, IRoomIdManager
    {
        public const int DefaultId = 0;

        public async Task<long> GenerateIdAsync()
        {
            if (false == State.Last.HasValue)
            {
                State.Last = 1;
            }
            else
            {
                State.Last++;
            }

            await WriteStateAsync();

            return State.Last.Value;
        }
    }
}