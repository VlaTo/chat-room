using Prism.Navigation;

namespace LibraProgramming.ChatRoom.Client.ViewModels
{
    public class PageViewModelBase : ViewModelBase
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public PageViewModelBase(INavigationService navigationService)
            : base(navigationService)
        {
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public override void OnNavigatingTo(INavigationParameters parameters)
        {
        }

        public override void Destroy()
        {
        }
    }
}