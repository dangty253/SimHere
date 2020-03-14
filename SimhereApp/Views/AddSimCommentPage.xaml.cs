
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace stuffnthings.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSimCommentPage : ContentPage
    {
        public SelectSimViewModel viewModel;
        public PostPageViewModel postPageViewModel;
        public AddSimCommentPage(PostPageViewModel postPageVM)
        {
            InitializeComponent();
            BindingContext = viewModel = new SelectSimViewModel(5);
            postPageViewModel = postPageVM;
            Title = "Sim của tôi";
            Initialize().GetAwaiter();
        }
        public async Task Initialize()
        {
           await viewModel.LoadData();
        }
        private void Discard_Clicked(object sender, EventArgs e)
        {
            Shell.Current.Navigation.PopAsync();
        }
        private async void Send_Clicked(object sender, EventArgs e)
        {
            try
            {
                var selected = viewModel.GetSelectedItems();
                if (selected.Count > 0)
                {
                    await postPageViewModel.AddComment(selected);

                    postPageViewModel.LoadOnRefreshCommand();
                    await Shell.Current.Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("", "Bạn cần chọn ít nhất 1 Sim.", "Đóng");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Phát sinh lỗi", ex.Message, "Đóng");
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var itemIndex = e.ItemIndex;
                viewModel.TappedItem(itemIndex);
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "OK");
            }
        }
        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var Sim = e.Item as SimViewModel;
            if (Sim.sim.Id == viewModel.Data.LastOrDefault().sim.Id)
                await viewModel.LoadMoreData();
        }
    }
}