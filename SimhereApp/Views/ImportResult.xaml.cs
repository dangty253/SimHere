using System;
using System.Collections.Generic;
using System.Linq;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ImportResult : ContentPage
    {
        public List<Sim> _simList { get; set; }
        public ImportResult(List<Sim> simLsit)
        {
            InitializeComponent();
            _simList = simLsit;

            SimListView.ItemsSource = _simList;

            SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                var Sim = e.Item as Sim;
                await this.Navigation.PushAsync(new PostSim(Sim.Id));
            };
            gridLoading.IsVisible = false;
        }

        public async void BtnDangSimClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Xác nhận", "Bạn có muốn bán tất cả sim vừa import không", "Đồng ý", "Đóng");
            if (!confirm) return;

            gridLoading.IsVisible = true;
            ApiResponse response = await ApiHelper.Put("api/sim/postsimlist", _simList.Select(x => x.Id).ToArray());
            if (response.IsSuccess)
            {                
                await Navigation.PopAsync(false);
                MySimListViewModel mySimListViewModel = Shell.Current.BindingContext as MySimListViewModel;
                mySimListViewModel.RefreshCommand.Execute(null);
                XFToast.ShortMessage("Đăng sim thành công");
            }
            else
            {
                await DisplayAlert("", response.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }
        public async void BtnDeleteSimClicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Xác nhận", "Bạn có muốn xóa sim đã import không", "Đồng ý", "Đóng");
            if (!confirm) return;

            gridLoading.IsVisible = true;
            ApiResponse response = await ApiHelper.Delete("api/sim?Ids=" + string.Join(",", _simList.Select(x => x.Id).ToArray()));
            if (response.IsSuccess)
            {
                await Navigation.PopAsync(false);
                XFToast.ShortMessage("Xoá sim thành công");
            }
            else
            {
                await DisplayAlert("", response.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }
    }
}
