using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SimHere.Entities;
using SimhereApp.Portable.Models;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class NumberTypesListToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return "Chọn loại sim";
            }

            ICollection<Sim_NumberType> numberTypes = (ICollection<Sim_NumberType>)value;
            if (numberTypes.Any() == false)
            {
                return "Chọn loại sim";
            }
            return string.Join(", ", numberTypes.Select(x => x.NumberType.Name).ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
