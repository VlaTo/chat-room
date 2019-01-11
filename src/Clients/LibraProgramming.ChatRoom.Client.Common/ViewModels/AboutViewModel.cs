using System;
using System.Windows.Input;
using LibraProgramming.ChatRoom.Client.Common.Core;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string title;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public ICommand OpenWebCommand
        {
            get;
        }

        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }
    }
}