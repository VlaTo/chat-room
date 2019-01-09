using System;
using System.Threading.Tasks;
using LibraProgramming.ChatRoom.Client.Common.Core;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Common.Views
{
    public class SetupAwareContentPage : ContentPage
    {
        private bool setupInvoked;

        public SetupAwareContentPage()
        {
            Appearing += DoAppearing;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            InvokeSetup();
        }

        private async void DoAppearing(object sender, EventArgs e)
        {
            Appearing -= DoAppearing;
            Disappearing += DoDisappearing;
            await InvokeSetup();
        }

        private async void DoDisappearing(object sender, EventArgs e)
        {
            Disappearing -= DoDisappearing;
            await InvokeCleanup();
        }

        private Task InvokeSetup()
        {
            if (setupInvoked)
            {
                return Task.CompletedTask;
            }

            if (BindingContext is IHasSetup model)
            {
                setupInvoked = true;
                return model.SetupAsync();
            }

            return Task.CompletedTask;
        }

        private Task InvokeCleanup()
        {
            if (false == setupInvoked)
            {
                return Task.CompletedTask;
            }

            if (BindingContext is IHasCleanup model)
            {
                return model.CleanupAsync();
            }

            return Task.CompletedTask;
        }
    }
}