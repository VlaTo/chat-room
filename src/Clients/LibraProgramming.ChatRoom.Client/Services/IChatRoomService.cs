using System.Collections.Generic;
using System.Security.Principal;
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
        /// <param name="roomId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Models.ChatRoom> GetRoomAsync(long roomId, CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Models.ChatRoom> CreateRoomAsync(string name, string description, CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<Models.ChatRoom> SaveRoomAsync(long roomId, string name, string description, CancellationToken ct);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="principal"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<IChatChannel> OpenChatAsync(long roomId, IPrincipal principal, CancellationToken ct);
    }
}