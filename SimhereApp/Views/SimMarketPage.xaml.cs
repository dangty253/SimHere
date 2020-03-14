using System;
using System.Linq;
using SimHere.Entities;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimMarketPage : ContentPage
    {
        PostListViewModel viewModel { get; set; }
        public SimMarketPage()
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

        async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var post = e.Item as Post;
            if (post.Type == 1)
                await Shell.Current.Navigation.PushAsync(new PostPage(post));
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