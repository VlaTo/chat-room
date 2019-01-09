using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Client.Common.Services
{
    public interface IChatRoomService
    {
        Task<IReadOnlyList<Models.ChatRoom>> GetRoomsAsync(CancellationToken ct);

        Task<Models.ChatRoom> SaveRoomAsync(string name, string description, CancellationToken ct);

        Task<IChatChannel> OpenChatAsync(long roomId, CancellationToken ct);
    }
}