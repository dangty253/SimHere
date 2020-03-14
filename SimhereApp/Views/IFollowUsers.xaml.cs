using System;
using System.Collections.Generic;
using System.Linq;
using SimHere.Entities;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class IFollowUsers : ContentPage
    {
        private IFollowUsersViewModel viewModel;
        private int type = 0;
        public IFollowUsers(int type)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new IFollowUsersViewModel(type);
            if (type == 1)
            {
                Title = "Người đang theo dõi bạn";
            }

            FollowListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                var item = e.Item as Users;
                await Navigation.PushAsync(new UserProfile(item.Id));
            };
            FollowListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                var user = e.Item as Users;
                if (user.Id == viewModel.Data.LastOrDefault().Id)
                    await viewModel.LoadMoreData();
            };
            Initialize();
        }
        public async void Initialize()
        {
            await viewModel.LoadData();
            viewModel.IsLoading = false;
        }
    }
}
