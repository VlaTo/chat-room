using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class NavigateToAction : TriggerAction
    {
        public NavigateToAction()
            : base(typeof(VisualElement))
        {
        }

        protected override void Invoke(object value)
        {
            throw new NotImplementedException();
        }
    }
}