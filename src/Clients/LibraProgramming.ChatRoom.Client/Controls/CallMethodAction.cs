using System;
using System.Reflection;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public sealed class CallMethodAction : TriggerAction
    {
        public static readonly BindableProperty MethodNameProperty;

        public string MethodName
        {
            get => (string) GetValue(MethodNameProperty);
            set => SetValue(MethodNameProperty, value);
        }

        public CallMethodAction()
            : base(typeof(VisualElement))
        {
        }

        static CallMethodAction()
        {
            MethodNameProperty = BindableProperty.Create(
                nameof(MethodName),
                typeof(string),
                typeof(CallMethodAction)
                //propertyChanged: OnMethodNamePropertyChanged
            );
        }

        protected override void Invoke(object value)
        {
            var type = AttachedObject.GetType();
            var methodInfo = type.GetMethod(MethodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            if (null == methodInfo)
            {
                return;
            }

            var args = methodInfo.GetParameters();
            var values = new object[args.Length];
            var t = value.GetType();

            for (var index = 0; index < args.Length; index++)
            {
                var parameterType = args[index].ParameterType;

                if (parameterType.IsAssignableFrom(t))
                {
                    values[index] = value;
                    continue;
                }

                if (parameterType.IsAssignableFrom(typeof(EventArgs)))
                {
                    values[index] = EventArgs.Empty;
                    continue;
                }

                throw new Exception();
            }

            methodInfo.Invoke(AttachedObject, values);
        }

        /*private static void OnMethodNamePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ;
        }*/
    }
}