using System;
using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain.Messages
{
    [DataContract]
    public sealed class OutgoingChatMessage
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

        [DataMember]
        public DateTime Created
        {
            get;
            set;
        }
    }
}
