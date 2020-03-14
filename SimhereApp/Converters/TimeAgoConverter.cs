using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using SimhereApp.Portable.Helpers;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class TimeAgoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    DateTime dateTime = DateTime.Parse(value.ToString());
                    return DateTimeHelper.TimeAgo(dateTime);
                }
                return value;
            }
            catch
            {
                return value;
            }
        }
           

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
