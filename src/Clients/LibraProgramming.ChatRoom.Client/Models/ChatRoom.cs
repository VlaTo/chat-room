namespace LibraProgramming.ChatRoom.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatRoom : IEntity<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get;
        }

        public ChatRoom(long id, string title, string description = null)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}