using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Controls
{
    public static class Interactivity
    {
        public static readonly BindableProperty RequestsProperty;

        static Interactivity()
        {
            RequestsProperty = BindableProperty.CreateAttached(
                "Requests",
                typeof(InteractionRequestTriggerCollection),
                typeof(Interactivity),
                default(InteractionRequestTriggerCollection),
                defaultValueCreator: bindable =>
                {
                    var collection = new InteractionRequestTriggerCollection();

                    collection.AttachTo(bindable);

                    return collection;
                }
            );
        }

        public static InteractionRequestTriggerCollection GetRequests(BindableObject bindable)
        {
            var collection = (InteractionRequestTriggerCollection) bindable.GetValue(RequestsProperty);

            if (null == collection)
            {
                collection = new InteractionRequestTriggerCollection();
                bindable.SetValue(RequestsProperty, collection);
            }

            return collection;
        }

        /*internal static void SetRequests(BindableObject bindable, IList<InteractionRequest> value)
        {
            bindable.SetValue(RequestsProperty, value);
        }*/
    }
}