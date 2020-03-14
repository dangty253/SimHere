using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using SimHere.Entities;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Helpers;

namespace SimhereApp.Portable.ViewModels
{
    public class SelectSimViewModel : ListViewPageViewModel<SimViewModel>
    {
        int numberOfSims;
        public SelectSimViewModel(int num)
        {
            numberOfSims = (int)num;
            Filter = new FilterModel() { OwnerId = UserLogged.Id, Status = 1 };
            PreLoadData = new Command(() =>
            {
                Filter.Keyword = SearchValue;
                string jsonStringFilterModel = JsonConvert.SerializeObject(Filter);
                ApiUrl = $"api/sim/filter?Page={Page}&filterModel={jsonStringFilterModel}";
            });
        }
        public List<SimViewModel> GetSelectedItems()
        {
            return Data.Where(x => x.IsChecked).ToList();
        }

        public void TappedItem(int itemIndex)
        {
            var checkedSims = GetSelectedItems();
            var tappedItem = Data[itemIndex];
            if (checkedSims.Count() < numberOfSims || checkedSims.Contains(tappedItem))
            {
                if (tappedItem.IsChecked)
                {
                    tappedItem.IsChecked = false;
                }
                else
                {
                    tappedItem.IsChecked = true;
                }
            }
            else
            {
                throw new Exception("Chỉ có thể chọn tối đa " + numberOfSims + " số sim.");
            }
        }
        public override async Task LoadData()
        {
            PreLoadData.Execute(null);
            var result = await ApiHelper.Get<List<Sim>>(ApiUrl, true);
            if (result.IsSuccess)
            {
                var list = (List<Sim>)result.Content;
                var count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        Data.Add(new SimViewModel(item));
                    }
                }
                else
                {
                    OutOfData = true;
                }
            }
            else
            {
                Data.Clear();
                Page = 1;
            }
            OnPropertyChanged(nameof(IsEmptyList));
        }
    }
}
