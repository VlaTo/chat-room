using System;
using System.Runtime.Serialization;

namespace LibraProgramming.Services.Chat.Domain.Messages
{
    [DataContract]
    public sealed class OutgoingChatMessage : MessageContentBase
    {
        [DataMember]
        public DateTime Created
        {
            get;
            set;
        }
    }
}
