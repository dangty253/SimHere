using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SimHere.Entities;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.StaticOptions;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class OptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    if (parameter !=null)
                    {
                        var options = OptionCenter.Get(parameter.ToString());
                        return options.SingleOrDefault(x => x.Key == int.Parse(value.ToString())).Value ?? "";
                    }
                    return string.Empty;
                }
                return string.Empty;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return string.Empty;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }
    }
}
