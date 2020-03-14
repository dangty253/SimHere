using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SimhereApp.Portable.ViewModels
{
    public class SearchSimViewModel : BaseViewModel
    {
        public ObservableCollection<Carrier> CarrierOptions { get; set; }
        public ObservableCollection<PrefixNumber> PrefixNumberOptions { get; set; }
        public ObservableCollection<SimTypeOption> NumberTypeOptions { get; set; }

        public SearchSimViewModel()
        {
            var test = CarrierData.Get();
            CarrierOptions = new ObservableCollection<Carrier>(CarrierData.Get());
            PrefixNumberOptions = new ObservableCollection<PrefixNumber>(PrefixNumberData.Get());
            NumberTypeOptions = new ObservableCollection<SimTypeOption>();
        }

        public async Task LoadNumberTypes()
        {
            var result = await ApiHelper.Get<List<NumberType>>("api/sim/numbertypes");
            if (result.IsSuccess)
            {
                var numberTypes = result.Content as List<NumberType>;
                var numberTypesCount = numberTypes.Count();
                for (int i = 0; i < numberTypesCount; i++)
                {
                    var numberType = numberTypes[i];

                    NumberTypeOptions.Add(new SimTypeOption(numberType.Name)
                    {
                        Id = numberType.Id,
                        Name = numberType.Name
                    });
                }
            }
            else
            {
                throw new Exception("không tim thấy dữ liệu");
            }
        }
    }
}
