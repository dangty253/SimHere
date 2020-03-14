using System;
using System.Globalization;
using Xamarin.Forms;
namespace SimhereApp.Portable.Converters
{
    public class PostDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int year = DateTime.Now.Year;
                DateTime dateTime = DateTime.Parse(value.ToString());
                if (year == dateTime.Year)
                {
                    var rs = dateTime.ToString("dd/MM");
                    return rs;
                }
                else
                {
                    return dateTime.ToString("dd/MM/yyyy");
                }
            }
            else
            {
                return DateTime.Now.ToString("dd/MM");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Now.ToString("dd/MM");
        }
    }
}
