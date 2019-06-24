using System.Collections.Generic;
using LibraProgramming.ChatRoom.Client.Models.Database;
using LibraProgramming.ChatRoom.Client.Persistence;

namespace LibraProgramming.ChatRoom.Client.Services
{
    public sealed class MessageService : IMessageService
    {
        private readonly ChatDatabaseContext context;

        public MessageService(ChatDatabaseContext context)
        {
            this.context = context;
        }

        public IEnumerable<Message> GetMessages()
        {
            using (var uof = new UnitOfWork(context))
            {
                return uof.Messages.GetAll();
            }
        }

        public void Save(Message message)
        {
            using (var uof = new UnitOfWork(context))
            {
                uof.Messages.Save(message);
                uof.Commit();
            }
        }
    }
}