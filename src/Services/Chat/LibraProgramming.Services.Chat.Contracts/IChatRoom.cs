using System;
using System.Threading.Tasks;
using LibraProgramming.Services.Chat.Contracts.Models;
using Orleans;

namespace LibraProgramming.Services.Chat.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChatRoom : IGrainWithIntegerKey
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<RoomDescription> GetDescriptionAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        Task SetDescriptionAsync(string description);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<Guid> JoinAsync();
    }
}