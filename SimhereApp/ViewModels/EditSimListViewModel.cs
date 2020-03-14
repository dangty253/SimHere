using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class EditSimListViewModel : ListViewPageViewModel<EditListSimModel>
    {
        public ObservableCollection<Carrier> CarrierOptions { get; set; }
        public ObservableCollection<SimTypeOption> NumberTypeOptions { get; set; }
        public EditSimListViewModel()
        {
            CarrierOptions = new ObservableCollection<Carrier>(CarrierData.Get());
            NumberTypeOptions = new ObservableCollection<SimTypeOption>();
            PreLoadData = new Command(() =>
            {
                ApiUrl = $"api/sim/myeditsims?Page={Page}";
            });
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
                        Id = numberType.Id
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
