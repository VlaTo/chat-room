using System.Collections.Generic;
using System.Linq;
using LibraProgramming.ChatRoom.Client.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace LibraProgramming.ChatRoom.Client.Persistence
{
    public sealed class MessageRepository : IRepository<long, Message>
    {
        private readonly DbSet<Message> collection;

        public MessageRepository(DbSet<Message> collection)
        {
            this.collection = collection;
        }

        public Message FindById(long id)
        {
            return collection
                .AsNoTracking()
                .FirstOrDefault(message => message.Id == id);
        }

        public IEnumerable<Message> GetAll()
        {
            return collection
                .AsNoTracking()
                .OrderBy(message => message.Id)
                .AsEnumerable();
        }

        public void Save(Message entity)
        {
            collection.Add(entity);
        }
    }
}