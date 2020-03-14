using SimhereApp.Portable.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;
namespace SimhereApp.Portable.Converters
{
    public class AvatarConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var x = AvatarHelper.ToAvatarUrl(value.ToString());
                return x;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}
