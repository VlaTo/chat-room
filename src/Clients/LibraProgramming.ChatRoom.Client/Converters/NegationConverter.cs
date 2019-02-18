using System;
using System.Globalization;
using Xamarin.Forms;

namespace LibraProgramming.ChatRoom.Client.Converters
{
    public sealed class NegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (typeof(bool) != targetType)
            {
                return value;
            }

            return false == System.Convert.ToBoolean(value, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
