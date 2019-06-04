using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public interface IAttachedObject
    {
        void AttachTo(BindableObject bindable);

        void DetachFrom(BindableObject bindable);
    }
}