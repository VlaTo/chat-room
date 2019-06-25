using System;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public interface IApiClient
    {
        Task<T> GetAsync<T>(Uri relativeUri);

        void SetBearerToken(string bearerToken);
    }
}