using Prism.Navigation;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Views
{
    public partial class PrismNavigationPage : INavigationPageOptions
    {
        public bool ClearNavigationStackOnNavigation => true;

        public PrismNavigationPage()
        {
            InitializeComponent();
        }
    }
}
