using System;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class InteractionRequestedEventArgs : EventArgs
    {
        public Action Callback
        {
            get;
        }

        public InteractionRequestedEventArgs(Action callback)
        {
            Callback = callback;
        }
    }
}