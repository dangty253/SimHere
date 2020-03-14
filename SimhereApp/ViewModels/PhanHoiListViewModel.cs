using System;
using SimHere.Entities;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class PhanHoiListViewModel : ListViewPageViewModel<SimHere.Entities.PhanHoi>
    {
        public PhanHoiListViewModel()
        {
            PreLoadData = new Command(() => ApiUrl = $"api/phanhoi");
        }
    }
}

