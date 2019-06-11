using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    [ContentProperty(nameof(Actions))]
    public abstract class InteractionRequestTriggerBase : InteractivityBase
    {
        public static readonly BindableProperty ActionsProperty;
        public static readonly BindableProperty RequestProperty;

        public TriggerActionCollection Actions
        {
            get => (TriggerActionCollection) GetValue(ActionsProperty);
        }

        public IInteractionRequest Request
        {
            get => (IInteractionRequest) GetValue(RequestProperty);
            set => SetValue(RequestProperty, value);
        }

        protected BindableObject AttachedObject
        {
            get;
            private set;
        }

        static InteractionRequestTriggerBase()
        {
            ActionsProperty = BindableProperty.Create(
                nameof(Actions),
                typeof(TriggerActionCollection),
                typeof(InteractionRequestTriggerBase),
                defaultValueCreator: _ => new TriggerActionCollection()
            );
            RequestProperty = BindableProperty.Create(
                nameof(Request),
                typeof(IInteractionRequest),
                typeof(InteractionRequestTriggerBase),
                propertyChanged: OnRequestPropertyChanged
            );
        }

        internal InteractionRequestTriggerBase(Type constraint)
            : base(constraint)
        {
        }

        public override void AttachTo(BindableObject bindable)
        {
            if (null != AttachedObject)
            {
                throw new InvalidOperationException("Cannot Host Trigger Multiple Times");
            }

            if (null != bindable && false == AttachedObjectTypeConstraint.IsInstanceOfType(bindable))
            {
                throw new InvalidOperationException("Type Constraint Violated");
            }

            AttachedObject = bindable;

            OnAssociatedObjectChanged();

            base.AttachTo(bindable);
            
            Actions.AttachTo(bindable);
            
            //OnAttached();
        }

        public override void DetachFrom(BindableObject bindable)
        {
            if (null == AttachedObject)
            {
                throw new InvalidOperationException("Cannot Host Trigger Multiple Times");
            }

            AttachedObject = null;

            base.DetachFrom(bindable);

            Actions.DetachFrom(bindable);
        }

        protected void InvokeActions(object parameter)
        {
            /*var handler = PreviewInvoke;

            if (null != handler)
            {
                var e = new CancelEventArgs(false);

                handler(this, e);

                if (e.Cancel)
                {
                    return;
                }
            }*/

            foreach (var trigger in Actions)
            {
                trigger.Call(parameter);
            }
        }

        private void OnRequestChanged(IInteractionRequest oldvalue, IInteractionRequest newvalue)
        {
            if (null != oldvalue)
            {
                oldvalue.Raised -= OnInteractionRequested;
            }

            if (null != newvalue)
            {
                newvalue.Raised += OnInteractionRequested;
            }
        }

        private void OnInteractionRequested(object sender, InteractionRequestedEventArgs e)
        {
            try
            {
                InvokeActions(e.Context);

            }
            finally
            {
                e.Callback.Invoke();
            }
        }

        private static void OnRequestPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((InteractionRequestTriggerBase) bindable).OnRequestChanged(
                (IInteractionRequest) oldvalue,
                (IInteractionRequest) newvalue
            );
        }
    }

    public abstract class InteractionRequestTriggerBase<T> : InteractionRequestTriggerBase
    {
        /*protected new T AttachedObject
        {
            get
            {
                return (T) base.AttachedObject;
            }
        }*/

        protected override sealed Type AttachedObjectTypeConstraint
        {
            get
            {
                return base.AttachedObjectTypeConstraint;
            }
        }

        protected InteractionRequestTriggerBase(Type constraint)
            : base(constraint)
        {
        }
    }
}
