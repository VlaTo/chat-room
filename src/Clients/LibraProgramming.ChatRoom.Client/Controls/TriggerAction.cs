using System;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public abstract class TriggerAction : InteractivityBase
    {
        public static readonly BindableProperty IsEnabledProperty;

        public bool IsEnabled
        {
            get => (bool) GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        internal bool IsHosted
        {
            get; 
            set;
        }

        protected TriggerAction(Type constraint)
        {
            AttachedObjectTypeConstraint = constraint;
        }

        static TriggerAction()
        {
            IsEnabledProperty = BindableProperty.Create(
                nameof(IsEnabled),
                typeof(bool),
                typeof(TriggerAction),
                defaultValue: true
            );
        }

        public override void AttachTo(BindableObject bindable)
        {
            /*if (bindable == AttachedObject)
            {
                return;
            }

            if (null != AttachedObject)
            {
                throw new InvalidOperationException("Cannot Host TriggerAction Multiple Times");
            }

//            if (null != element && !AttachedObjectTypeConstraint.GetTypeInfo().IsAssignableFrom(element.GetType().GetTypeInfo()))
            if (null != bindable && !AttachedObjectTypeConstraint.IsInstanceOfType(bindable))
            {
                throw new InvalidOperationException("Type Constraint Violated");
            }
            
            AttachedObject = bindable;

            OnAssociatedObjectChanged();

            base.Attach(bindable);
            
            OnAttached();*/
        }

        public override void DetachFrom(BindableObject bindable)
        {
            /*base.Detach();

            OnDetaching();

            AttachedObject = null;

            OnAssociatedObjectChanged();*/
        }

        internal void Call(object value)
        {
            if (IsEnabled)
            {
                Invoke(value);
            }
        }

        protected abstract void Invoke(object value);

    }
}