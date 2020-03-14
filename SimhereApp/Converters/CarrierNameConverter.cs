using SimHere.Entities;
using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class CarrierNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || (short)value == 0) return "Chọn nhà mạng";
            Int16 CarrierId = (Int16)value;
            return CarrierData.Get().SingleOrDefault(x => x.Id == CarrierId).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
