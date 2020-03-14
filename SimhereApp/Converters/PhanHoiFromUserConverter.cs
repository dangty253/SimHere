using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class PhanHoiFromUserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Sim Here";
            }
            else
            {
                return (value as SimHere.Entities.Users).FullName;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
