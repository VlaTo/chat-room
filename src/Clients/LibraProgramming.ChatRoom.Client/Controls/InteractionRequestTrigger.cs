using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class InteractionRequestTrigger<TRequestContext> : InteractionRequestTriggerBase<TRequestContext>
        where TRequestContext : InteractionRequestContext
    {
        public InteractionRequestTrigger()
            : base(typeof(VisualElement))
        {
        }
    }
}