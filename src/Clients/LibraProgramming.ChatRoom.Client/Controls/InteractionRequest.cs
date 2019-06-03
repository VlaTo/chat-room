using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class InteractionRequest : BindableObject, IInteractionRequest
    {
        public event EventHandler<InteractionRequestedEventArgs> Raised;

        public InteractionRequest()
        {
        }

        protected void DoRaiseEvent(InteractionRequestedEventArgs e)
        {
            var handler = Raised;

            if (null != handler)
            {
                handler.Invoke(this, e);
            }
        }
    }

    public class InteractionRequest<TInteraction> : InteractionRequest
        where TInteraction : InteractionRequestContext
    {
        public void Raise(TInteraction interaction, Action callback)
        {
            DoRaiseEvent(new InteractionRequestedEventArgs(callback));
        }
    }
}