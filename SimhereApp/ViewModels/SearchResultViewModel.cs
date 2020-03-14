using Newtonsoft.Json;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace SimhereApp.Portable.ViewModels
{
    public class SearchResultViewModel : ListViewPageViewModel<Sim>
    {
        private FilterModel FilterModel;
        public SearchResultViewModel(FilterModel filterModel)
        {
            FilterModel = filterModel;
            PreLoadData = new Command(() =>
            {
                string jsonStringFilterModel = JsonConvert.SerializeObject(FilterModel);
                ApiUrl = $"api/sim/filter?Page={Page}&filterModel={jsonStringFilterModel}";
            });
        }
    }
}

