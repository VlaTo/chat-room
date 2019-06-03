using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public class BehaviorBase<T> : Behavior<T>
        where T : BindableObject
    {
        public T AttachedObject
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(T bindable)
        {
            base.OnAttachedTo(bindable);

            AttachedObject = bindable;

            if (null != bindable.BindingContext)
            {
                BindingContext = bindable.BindingContext;
            }

            bindable.BindingContextChanged += OnBindingContextChanged;
        }

        protected override void OnDetachingFrom(T bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= OnBindingContextChanged;

            AttachedObject = null;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            BindingContext = AttachedObject.BindingContext;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}