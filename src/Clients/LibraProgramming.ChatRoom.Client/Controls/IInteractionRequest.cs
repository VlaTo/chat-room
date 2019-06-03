using System;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public interface IInteractionRequest
    {
        event EventHandler<InteractionRequestedEventArgs> Raised;
    }
}