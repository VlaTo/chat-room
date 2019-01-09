using System;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraProgramming.ChatRoom.Client.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomsPage
    {
        public RoomsPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem is ChatRoomViewModel room)
            {
                var viewModel = (RoomsViewModel) BindingContext;
                await Navigation.PushAsync(new ChatPage(new ChatViewModel(viewModel.ChatRoomService, room)));
            }

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        private async void OnAddRoomClicked(object sender, EventArgs e)
        {
            var room = new ChatRoomViewModel(-1L)
            {
                Title = "",
                Description = ""
            };

            await Navigation.PushModalAsync(new NavigationPage(new NewRoomPage(room)));
        }
    }
}