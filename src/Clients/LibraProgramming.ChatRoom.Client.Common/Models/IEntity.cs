namespace LibraProgramming.ChatRoom.Client.Common.Models
{
    public interface IEntity<out TId>
    {
        TId Id
        {
            get;
        }
    }
}