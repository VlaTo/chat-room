namespace LibraProgramming.ChatRoom.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    public interface IEntity<out TId>
    {
        /// <summary>
        /// 
        /// </summary>
        TId Id
        {
            get;
        }
    }
}