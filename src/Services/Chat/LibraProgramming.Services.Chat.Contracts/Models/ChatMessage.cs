using System;
using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Contracts.Models
{
    [DataContract]
    [Serializable]
    public sealed class ChatMessage
    {
        [DataMember]
        public DateTime Created
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
