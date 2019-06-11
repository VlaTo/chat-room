using System;
using LibraProgramming.ChatRoom.Client.Controls;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class NewMessageContext : InteractionRequestContext
    {
        public Action MessageCallback
        {
            get;
        }

        public NewMessageContext(Action messageCallback)
        {
            MessageCallback = messageCallback;
        }
    }
}