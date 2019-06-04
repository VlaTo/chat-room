using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class InteractionRequest : BindableObject, IInteractionRequest, IAttachedObject
    {
        //public static readonly BindableProperty 
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

        public void AttachTo(BindableObject bindable)
        {
            ;
        }

        public void DetachFrom(BindableObject bindable)
        {
            ;
        }
    }

    public class InteractionRequest<TInteraction> : InteractionRequest
        where TInteraction : InteractionRequestContext
    {
        public void Raise(TInteraction interaction, Action callback)
        {
            if (null == interaction)
            {
                throw new ArgumentNullException(nameof(interaction));
            }

            DoRaiseEvent(new InteractionRequestedEventArgs(interaction, callback.Invoke));
        }
    }
}