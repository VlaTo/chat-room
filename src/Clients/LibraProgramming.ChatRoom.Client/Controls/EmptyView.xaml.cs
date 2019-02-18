using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmptyView : ContentView
    {
        public EmptyView()
        {
            InitializeComponent();
        }
    }
}