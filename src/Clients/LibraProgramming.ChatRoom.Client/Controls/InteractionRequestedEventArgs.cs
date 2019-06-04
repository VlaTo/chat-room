using System;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class InteractionRequestedEventArgs : EventArgs
    {
        public InteractionRequestContext Context
        {
            get;
        }

        public Action Callback
        {
            get;
        }

        public InteractionRequestedEventArgs(InteractionRequestContext context, Action callback)
        {
            Context = context;
            Callback = callback;
        }
    }
}