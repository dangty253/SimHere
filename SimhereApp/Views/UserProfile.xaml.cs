using System.Linq;
using Xamarin.Forms;
using SimHere.Entities;
using SimhereApp.Portable.Settings;
using System.ComponentModel;
using SimhereApp.Portable.Controls;
using System;
using SimhereApp.Portable.ViewModels;

namespace SimhereApp.Portable.Views
{
    public partial class UserProfile : ContentPage
    {
        public readonly UserProfileViewModel viewModel;
        public UserProfile(string userId)
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UserProfileViewModel(userId);

            SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                var item = e.Item as Sim;
                await Shell.Current.Navigation.PushAsync(new SimDetail(item.Id));
            };

            SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                var Sim = e.Item as Sim;
                if (Sim.Id == viewModel.Data.LastOrDefault().Id)
                {
                    await viewModel.LoadMoreData();
                }
            };
        }
    }
}

