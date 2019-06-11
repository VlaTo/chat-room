using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public abstract class InteractivityBase : BindableObject, IAttachedObject
    {
        protected virtual Type AttachedObjectTypeConstraint
        {
            get;
        }

        internal event EventHandler AttachedObjectChanged;

        protected InteractivityBase(Type attachedObjectTypeConstraint)
        {
            AttachedObjectTypeConstraint = attachedObjectTypeConstraint;
        }

        public virtual void AttachTo(BindableObject bindable)
        {
            if (null != bindable)
            {
                bindable.BindingContextChanged += DoBindingContextChanged;

                if (null != bindable.BindingContext)
                {
                    DoBindingContextChanged(bindable);
                }
            }
        }

        public virtual void DetachFrom(BindableObject bindable)
        {
            if (null != bindable)
            {
                bindable.BindingContextChanged -= DoBindingContextChanged;
            }
        }

        protected void OnAssociatedObjectChanged()
        {
            var handler = AttachedObjectChanged;

            if (null == handler)
            {
                return;
            }

            handler(this, EventArgs.Empty);
        }

        private void DoBindingContextChanged(BindableObject bindable)
        {
            if (null != bindable)
            {
                SetBinding(BindingContextProperty,
                    new Binding
                    {
                        Path = nameof(BindingContext),
                        Source = bindable
                    });

                OnBindingContextChanged();
            }
        }

        private void DoBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }
    }
}
