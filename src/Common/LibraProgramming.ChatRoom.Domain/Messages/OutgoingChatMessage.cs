using System;
using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Domain.Messages
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
