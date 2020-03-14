using SimHere.Entities;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Follow : ContentPage
    {
        private SimFollowViewModel viewModel;
        public Follow()
        {
            InitializeComponent();

            SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) => {
                var item = e.Item as Sim;
                await Navigation.PushAsync(new SimDetail(item.Id));
            };

            SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) => {
                var Sim = e.Item as Sim;
                if (Sim.Id == viewModel.Data.LastOrDefault().Id)
                {
                    await viewModel.LoadMoreData();
                }
            };
            Initialized();
        }
        public async void Initialized()
        {
            this.BindingContext = viewModel = new SimFollowViewModel();

            if (UserLogged.IsLogged)
            {
                await viewModel.LoadData();
                stackLayoutBtnLogin.IsVisible = false;
            }
            else
            {
                stackLayoutBtnLogin.IsVisible = true;
                btnLogin.Clicked += (sender, e) =>
                {
                    Shell.Current.GoToAsync("//homes/account");
                };
                lblEmptyList.IsVisible = false;
            }
            gridLoading.IsVisible = false;
        }
    }
}