using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain.Messages
{
    [DataContract]
    public class MessageContentBase
    {
        [DataMember]
        public string Author
        {
            get;
            set;
        }

        [DataMember]
        public string Content
        {
            get;
            set;
        }
    }
}