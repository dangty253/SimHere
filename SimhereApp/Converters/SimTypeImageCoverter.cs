using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using SimHere.Entities;
using SimhereApp.Portable.Configuration;
using Xamarin.Forms;

namespace SimhereApp.Portable.Converters
{
    public class SimTypeImageCoverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var sim = value as Sim;
                    if (sim.Sim_NumberTypes.Count > 0)
                    {
                        var check = true;
                        while (check)
                        {
                            var numberType = sim.Sim_NumberTypes.FirstOrDefault();
                            if (numberType.NumberTypeId == 1 || numberType.NumberTypeId == 21)
                            {
                                sim.Sim_NumberTypes.Remove(numberType);
                            }
                            else
                            {
                                string imageName = Constants.SimDetailBackgroundDictionary.keys[numberType.NumberTypeId];
                                if (imageName == "")
                                {
                                    return "sim1.jpg";
                                }
                                else
                                {
                                    return AppConfig.API_IP + "HinhLoaiSim/" + imageName;
                                }
                                check = false;
                            }
                        };
                    }

                }
                return "sim1.jpg";
            }
            catch
            {
                return "sim1.jpg";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }
}
