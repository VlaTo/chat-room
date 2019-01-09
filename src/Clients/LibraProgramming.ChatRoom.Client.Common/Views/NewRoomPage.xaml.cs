using System;
using LibraProgramming.ChatRoom.Client.Common.Models;
using LibraProgramming.ChatRoom.Client.Common.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraProgramming.ChatRoom.Client.Common.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRoomPage
    {
        public ChatRoomViewModel ChatRoom => (ChatRoomViewModel) BindingContext;

        public NewRoomPage(ChatRoomViewModel model)
        {
            InitializeComponent();

            BindingContext = model;
        }

        async void OnSaveRoomClicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(
                this,
                "AddNewRoom",
                new AddNewRoomMessage(ChatRoom.Title, ChatRoom.Description)
            );

            await Navigation.PopModalAsync();
        }
    }
}