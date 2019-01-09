using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace LibraProgramming.Services.Chat.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChatRoomResolver : IGrainWithIntegerKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyDictionary<long, string>> GetRoomsAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<long> RegisterRoomAsync(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> RenameRoomAsync(long id, string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveRoomAsync(long id);
    }
}