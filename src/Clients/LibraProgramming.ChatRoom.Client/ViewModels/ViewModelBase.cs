using Prism.Mvvm;
using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public abstract class ViewModelBase : BindableBase, INavigationAware, IDestructible
    {
        private string _title;

        protected INavigationService NavigationService
        {
            get;
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
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
