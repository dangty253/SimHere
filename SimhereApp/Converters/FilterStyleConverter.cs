using SimhereApp.Portable.Helpers;
using System;
using System.Globalization;
using Xamarin.Forms;
namespace SimhereApp.Portable.Converters
{
    public class FilterStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && (bool)value)
            {
                return (Style)Application.Current.Resources["FilterSelectedButton"];
            }
            return (Style)Application.Current.Resources["FilterButton"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Style)Application.Current.Resources["FilterButton"];
        }
    }
}
