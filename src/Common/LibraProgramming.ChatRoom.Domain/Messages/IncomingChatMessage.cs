using System.Runtime.Serialization;

namespace LibraProgramming.ChatRoom.Domain.Messages
{
    [DataContract]
    public sealed class IncomingChatMessage : MessageContentBase
    {
    }
}