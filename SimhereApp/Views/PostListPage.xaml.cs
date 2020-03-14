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
    public partial class PostListPage : ContentPage
    {
        PostListViewModel viewModel { get; set; }
        public PostListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new PostListViewModel();
        }
        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (((Post)e.Item).Id == viewModel.Data.LastOrDefault().Id)
            {
                await viewModel.LoadMoreData();
            }
        }
        private async void User_Tapped(object sender, EventArgs e)
        {
            try
            {
                var g = sender as StackLayout;
                var tap = g.GestureRecognizers[0] as TapGestureRecognizer;
                var id = tap.CommandParameter as string;
                if (!string.IsNullOrWhiteSpace(id))
                    await Shell.Current.Navigation.PushAsync(new UserProfile(id));
            }
            catch { }
        }
        private void Login_Tapped(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//homes/account");
        }
    }
}