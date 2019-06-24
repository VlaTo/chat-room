using System;
using LibraProgramming.ChatRoom.Client.Models.Database;

namespace LibraProgramming.ChatRoom.Client.Persistence
{
    public sealed class UnitOfWork : IDisposable
    {
        private readonly ChatDatabaseContext context;

        public IRepository<long, Message> Messages
        {
            get;
        }

        public UnitOfWork(ChatDatabaseContext context)
        {
            this.context = context;
            Messages = new MessageRepository(context.Messages);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            //context;
        }
    }
}