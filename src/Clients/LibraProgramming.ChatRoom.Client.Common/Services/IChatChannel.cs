using System;
using System.Threading.Tasks;

namespace LibraProgramming.ChatRoom.Client.Common.Services
{
    public sealed class ChatMessageEventArgs : EventArgs
    {
        public string Text
        {
            get;
        }

        public ChatMessageEventArgs(string text)
        {
            Text = text;
        }
    }

    public interface IChatChannel : IDisposable
    {
        event EventHandler<ChatMessageEventArgs> MessageArrived;

        Task SendAsync(string text);
    }
}