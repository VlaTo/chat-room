using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain.Messages
{
    [DataContract]
    public sealed class IncomingChatMessage
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