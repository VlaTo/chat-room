using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using Xamarin.Forms.Xaml;

namespace LibraProgramming.ChatRoom.Client.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage
    {
        public ChatPage(ChatViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}