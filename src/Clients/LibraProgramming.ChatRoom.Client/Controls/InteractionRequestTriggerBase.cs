using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    [ContentProperty(nameof(Actions))]
    public abstract class InteractionRequestTriggerBase : InteractivityBase
    {
        public static readonly BindableProperty ActionsProperty;

        public TriggerActionCollection Actions
        {
            get
            {
                return (TriggerActionCollection) GetValue(ActionsProperty);
            }
        }

        static InteractionRequestTriggerBase()
        {
            ActionsProperty = BindableProperty.Create(
                nameof(Actions),
                typeof(TriggerActionCollection),
                typeof(InteractionRequestTriggerBase)
            );
        }

        internal InteractionRequestTriggerBase(Type constraint)
        {
            AttachedObjectTypeConstraint = constraint;
            
            var actionCollection = new TriggerActionCollection();
            
            SetValue(ActionsProperty, actionCollection);
        }

        public override void AttachTo(BindableObject bindable)
        {
            if (null == bindable)
            {
                throw new InvalidOperationException("Cannot Host Trigger Multiple Times");
            }

            /*if (null != bindable && !AttachedObjectTypeConstraint.GetTypeInfo().IsAssignableFrom(bindable.GetType().GetTypeInfo()))
            {
                throw new InvalidOperationException("Type Constraint Violated");
            }

            AttachedObject = bindable;*/

            OnAssociatedObjectChanged();

            //Attach handles the DataContext
            base.AttachTo(bindable);
            
            Actions.AttachTo(bindable);
            
            //OnAttached();
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
