using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SimhereApp.Portable.Settings;
using Telerik.XamarinForms.Input;
using SimHere.Entities.Response;
using System.Threading.Tasks;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MySimList : ContentPage
    {
        private MySimListViewModel viewModel;

        public MySimList()
        {
            InitializeComponent();
            Initialize();
        }
    
        public async Task Initialize()
        {
            this.BindingContext = viewModel = new MySimListViewModel();
            if (UserLogged.IsLogged)
            {
                //BtnAddVisible.IsVisible = true;
                //BtnEdit.IsVisible = true;
                //BtnBuyAll.IsVisible = true;
                await viewModel.LoadData();
                ScrollViewStatus.IsVisible = true;

                SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
                {
                    var item = e.Item as Sim;
                    await Shell.Current.Navigation.PushAsync(new PostSim(item.Id));
                };

                SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
                {
                    var Sim = e.Item as Sim;
                    if (Sim.Id == viewModel.Data.LastOrDefault().Id)
                        await viewModel.LoadMoreData();
                };

                MessagingCenter.Subscribe<PostSim, string>(this, "DeleteSim", (postSimForm, SimId) =>
                {
                    var simDeleted = viewModel.Data.SingleOrDefault(x => x.Id == SimId);
                    if (simDeleted != null)
                    {
                        viewModel.Data.Remove(simDeleted);
                    }
                });
            }
          
        }

        private void ChangeStatus_Clicked(object sender, EventArgs e)
        {
            // clear border color of all button
            foreach (var item in StackLayoutButtonStatusGroup.Children)
            {
                ((RadButton)item).BorderThickness = 0;
            }
            // set border color for current button.
            ((RadButton)sender).BorderThickness = new Thickness(0, 0, 0, 3);
        }

        private async void BtnOpenNew_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserLogged.Phone))
            {
                await DisplayAlert("", "Vui lòng cập nhật số điện thoại trước khi tạo sim", "Đóng");
                return;
            }
            await Shell.Current.Navigation.PushAsync(new PostSim());
        }

        private async void ToolBarImport_Clicked(object sendr, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new AddSimByText());
        }

        //public async void AddSim_Clicked(object sender, EventArgs e)
        //{
        //    var actionSheet = await DisplayActionSheet("Thêm sim", "Huỷ", null, "Thêm", "Import từ Text");
        //    if (actionSheet == "Thêm")
        //    {
        //        if (string.IsNullOrEmpty(UserLogged.Phone))
        //        {
        //            await DisplayAlert("", "Vui lòng cập nhật số điện thoại trước khi tạo sim", "Đóng");
        //            return;
        //        }
        //        await Shell.Current.Navigation.PushAsync(new PostSim());
        //    }
        //    else if (actionSheet == "Import từ Text")
        //    {
        //        if (string.IsNullOrEmpty(UserLogged.Phone))
        //        {
        //            await DisplayAlert("", "Vui lòng cập nhật số điện thoại trước khi tạo sim", "Đóng");
        //            return;
        //        }
        //        await Shell.Current.Navigation.PushAsync(new AddSimByText());
        //    }
        //}

        //public async void GoToEdit_Clicked(object sender, EventArgs e)
        //{
        //    if ((viewModel.Status.HasValue == false || viewModel.Status == 0) && !viewModel.Data.Any())
        //    {
        //        await DisplayAlert("", "Không có sim chưa bán để sửa", "Đóng");
        //    }
        //    else
        //    {
        //        await Shell.Current.Navigation.PushAsync(new EditListSim());
        //    }
        //}

        //public async void BuyAll_Clicked(object sender, EventArgs e)
        //{
        //    var confirm = await DisplayAlert("Xác nhận", "Bạn có muốn đăng bán tất cả sim không", "Đồng ý", "Đóng");
        //    if (!confirm) return;
        //    ApiResponse response = await ApiHelper.Put("api/sim/buyall", null);
        //    if (response.IsSuccess)
        //    {
        //        viewModel.RefreshCommand.Execute(null);
        //        await DisplayAlert("", response.Message, "Đóng");
        //    }
        //    else
        //    {
        //        await DisplayAlert("", response.Message, "Đóng");
        //    }
        //}
        private void Login_Tapped(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//homes/account");
        }
    }
}