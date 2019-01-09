using System.Threading.Tasks;
using Orleans;

namespace LibraProgramming.Services.Chat.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRoomIdManager : IGrainWithIntegerKey
    {
        Task<long> GenerateIdAsync();
    }
}