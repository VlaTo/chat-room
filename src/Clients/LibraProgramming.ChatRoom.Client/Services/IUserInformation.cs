using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public interface IUserInformation
    {
        Task<string> GetUserNameAsync();
    }
}