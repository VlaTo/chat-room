namespace LibraProgramming.ChatRoom.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AddNewRoomMessage
    {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        public AddNewRoomMessage(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}