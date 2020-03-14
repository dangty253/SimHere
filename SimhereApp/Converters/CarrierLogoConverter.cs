using System;
using System.Globalization;
using Xamarin.Forms;
namespace SimhereApp.Portable.Converters
{
    public class CarrierLogoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Int16 CarrierId = (Int16)value;
            switch (CarrierId)
            {
                case 0:
                    return "https://via.placeholder.com/150x100.png/eeeeee/444444?text=Simhere";
                case 1:
                    return "viettel.png";
                case 2:
                    return "mobifone.png";
                case 3:
                    return "vinaphone.png";
                case 4:
                    return "vietnamobile.png";
                case 5:
                    return "gmobile.png";
                default:
                    return "";
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
