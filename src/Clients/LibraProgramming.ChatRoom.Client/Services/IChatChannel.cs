using System;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Domain.Messages;

namespace LibraProgramming.ChatRoom.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ChatMessageEventArgs : EventArgs
    {
        public OutgoingChatMessage Message
        {
            get;
        }

        public ChatMessageEventArgs(OutgoingChatMessage message)
        {
            Message = message;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IChatChannel : IDisposable
    {
        event EventHandler<ChatMessageEventArgs> MessageArrived;

        Task SendAsync(string text);
    }
}