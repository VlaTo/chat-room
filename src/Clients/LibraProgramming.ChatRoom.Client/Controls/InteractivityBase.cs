using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public abstract class InteractivityBase : BindableObject, IAttachedObject
    {
        protected virtual Type AttachedObjectTypeConstraint
        {
            get;
            set;
        }

        internal event EventHandler AttachedObjectChanged;
        
        public virtual void AttachTo(BindableObject bindable)
        {
            if (null != bindable)
            {
                //bindable.AddDataContextChangedHandler(BindingContextChanged);

                if (null != bindable.BindingContext)
                {
                    DoBindingContextChanged(bindable, EventArgs.Empty);
                }
            }
        }

        public virtual void DetachFrom(BindableObject bindable)
        {
            if (null != bindable)
            {
                //AttachedObject.RemoveDataContextChangedHandler(BindingContextChanged);
            }
        }

        protected void OnAssociatedObjectChanged()
        {
            var handler = AttachedObjectChanged;

            if (null == handler)
            {
                return;
            }

            handler(this, new EventArgs());
        }

        private void DoBindingContextChanged(object sender, EventArgs e)
        {
            /*var element = sender as FrameworkElement;

            if (null != element)
            {
                var dataContext = DataContext;

                SetBinding(DataContextProperty,
                    new Binding
                    {
                        Path = new PropertyPath("DataContext"),
                        Source = element
                    });

                OnDataContextChanged(dataContext, DataContext);
            }*/
        }
    }
}
