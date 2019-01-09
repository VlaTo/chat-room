using Ninject;

namespace LibraProgramming.ChatRoom.Client.Common.ViewModels
{
    public class ViewModelLocator
    {
        public RoomsViewModel RoomsPageViewModel => App.Kernel.Get<RoomsViewModel>();
    }
}