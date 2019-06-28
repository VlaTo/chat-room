using System.Collections.Generic;
using LibraProgramming.ChatRoom.Client.Models.Database;

namespace LibraProgramming.ChatRoom.Client.Persistence
{
    public interface IRepository<in TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        TEntity FindById(TKey id);

        IEnumerable<TEntity> GetAll();

        void Save(TEntity entity);
    }
}