using System;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class CreateNewPostPage : ContentPage
    {
        public CreateNewPostViewModel viewModel;
        public CreateNewPostPage(PostListViewModel PostListVM, int statusCode)
        {
            InitializeComponent();
            BindingContext = viewModel = new CreateNewPostViewModel(PostListVM, statusCode);
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var itemIndex = e.ItemIndex;
                viewModel.selectSimViewModel.TappedItem(itemIndex);
            }
            catch (Exception ex)
            {
                DisplayAlert("", ex.Message, "Đóng");
            }
        }
        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                var index = e.ItemIndex;
                if (index == viewModel.selectSimViewModel.Data.Count - 1)
                    await viewModel.selectSimViewModel.LoadMoreData();
            }
            catch (Exception ex)
            {
              await  DisplayAlert("", ex.Message, "Đóng");
            }
        }
    }
}
