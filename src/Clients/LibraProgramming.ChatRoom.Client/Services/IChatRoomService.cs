using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChatRoomService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IReadOnlyList<Models.ChatRoom>> GetRoomsAsync(CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Models.ChatRoom> SaveRoomAsync(string name, string description, CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IChatChannel> OpenChatAsync(long roomId, CancellationToken ct);
    }
}