using System;
using System.Globalization;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class CarrierTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int16 CarrierId = (Int16)value;
            switch (CarrierId)
            {
                case 1:
                    return Color.FromHex("#017066");
                case 2:
                    return Color.FromHex("#ED363E");
                case 3:
                    return Color.FromHex("#00A1E4");
                case 4:
                    return Color.FromHex("#FB5901");
                case 5:
                    return Color.FromHex("#FFC537");
                default:
                    return Color.Black;
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
