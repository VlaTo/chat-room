using System.Diagnostics;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Common
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
        {
            Debug.WriteLine($"[AppShell.OnShellNavigating] Target: {e.Target.Location}");
        }
    }
}
