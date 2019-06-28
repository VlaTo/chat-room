using System.Collections.Generic;
using LibraProgramming.ChatRoom.Client.Models.Database;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();

        void Save(Message message);
    }
}