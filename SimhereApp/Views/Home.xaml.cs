using SimHere.Entities;
using SimhereApp.Portable.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public HomeViewModel viewModel;
        public Home()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeViewModel();
        }
        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (((Sim)e.Item).Id == viewModel.Data.LastOrDefault().Id)
            {
                await viewModel.LoadMoreData();
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ReloadHotSims.Execute(null);
        }
    }
}