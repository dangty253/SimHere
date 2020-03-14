using SimHere.Entities;
using SimhereApp.Portable.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResult : ContentPage
    {
        private readonly SearchResultViewModel viewModel;
        public SearchResult(FilterModel filerModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new SearchResultViewModel(filerModel);
            
            SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                var item = e.Item as Sim;
                await Navigation.PushAsync(new SimDetail(item.Id));
            };

            SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                var Sim = e.Item as Sim;
                if (Sim.Id == viewModel.Data.LastOrDefault().Id)
                    await viewModel.LoadMoreData();
            };

            Initialize();
        }

        public async void Initialize()
        {
            await viewModel.LoadData();
            gridLoading.IsVisible = false;
        }

        private async void CloseSearchToolbar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}