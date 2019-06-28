namespace LibraProgramming.ChatRoom.Client.Models.Database
{
    public interface IEntity
    {

    }

    public interface IEntity<out TKey> : IEntity
    {
        TKey Id
        {
            get;
        }
    }
}