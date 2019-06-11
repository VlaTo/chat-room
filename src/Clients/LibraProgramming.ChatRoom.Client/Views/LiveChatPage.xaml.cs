using LibraProgramming.ChatRoom.Client.ViewModels;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Views
{
    public partial class LiveChatPage
    {
        public LiveChatPage()
        {
            InitializeComponent();
        }

        private void OnNewMessageInteractionRequested(NewMessageContext context)
        {
            Device.BeginInvokeOnMainThread(() => context.MessageCallback.Invoke());
        }
    }
}
