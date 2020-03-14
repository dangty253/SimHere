
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using stuffnthings.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostPage : ContentPage
    {
        PostPageViewModel viewModel { get; set; }

        public PostPage(Post post)
        {
            InitializeComponent();
            BindingContext = viewModel = new PostPageViewModel(post);
            Init();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.WatchingThisPost();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.StopWatchingThisPost();
        }
        public async Task Init()
        {
            await viewModel.LoadData();
        }
        private async void Lv_Comments_ItemAppearingAsync(object sender, ItemVisibilityEventArgs e)
        {
            if (((CommentModel)e.Item).Id == viewModel.Data.LastOrDefault().Id)
            {
                await viewModel.LoadMoreData();
            }
        }
        private void AddNewComment_Clicked(object sender, EventArgs e)
        {
            if (UserLogged.IsLogged)
            {
                if (UserLogged.Id != viewModel.MainPost.User.Id)
                {
                    Shell.Current.Navigation.PushAsync(new AddSimCommentPage(viewModel));
                }
                else
                {
                    DisplayAlert("", "Bạn không thể đăng tin trên bài viết của mình.", "Đóng");
                }
            }
            else
            {
                DisplayAlert("", "Bạn cần đăng nhập để thực hiện chức năng này.", "Đóng");
            }
        }

        private void Sim_ItemTapped(object sender, EventArgs e)
        {
            var st = sender as StackLayout;
            var tap = st.GestureRecognizers[0] as TapGestureRecognizer;
            var item = tap.CommandParameter as SimLite;
            Shell.Current.Navigation.PushAsync(new SimDetail(item.Id, item.Price));
        }

        private void User_Tapped(object sender, EventArgs e)
        {
            try
            {
                var g = sender as StackLayout;
                var tap = g.GestureRecognizers[0] as TapGestureRecognizer;
                var id = tap.CommandParameter as string;
                if (!string.IsNullOrWhiteSpace(id))
                    Shell.Current.Navigation.PushAsync(new UserProfile(id));
            }
            catch { }
        }
    }
}