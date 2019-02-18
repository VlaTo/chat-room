using Prism.Mvvm;
using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService
        {
            get;
        }

        protected ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public abstract void OnNavigatedFrom(INavigationParameters parameters);

        public abstract void OnNavigatedTo(INavigationParameters parameters);

        public abstract void OnNavigatingTo(INavigationParameters parameters);

        public abstract void Destroy();
    }
}
