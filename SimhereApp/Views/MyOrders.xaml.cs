using System;
using System.Collections.Generic;
using System.Linq;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;
using SimHere.Entities;
using Telerik.XamarinForms.Input;

namespace SimhereApp.Portable.Views
{
    public partial class MyOrders : ContentPage
    {
        private readonly MyOrdersViewModel viewModel;
        public MyOrders()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new MyOrdersViewModel();
            SimListView.ItemTapped += async (object sender, ItemTappedEventArgs e) =>
            {
                var item = e.Item as SimHere.Entities.Order;
                await Navigation.PushAsync(new OrderPage(item.OrderId.ToString()));
            };

            SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                var Sim = e.Item as SimHere.Entities.Order;
                if (Sim.OrderId == viewModel.Data.LastOrDefault().OrderId)
                {
                    await viewModel.LoadMoreData();
                }
            };
            Initialize();
        }

        public async void Initialize()
        {
            BtnDonHangMua.BorderWidth = 1;
            viewModel.OrderType = 0;
            await viewModel.LoadData();
            gridLoaing.IsVisible = false;
        }

        private void DonHangMua_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OrderType == 0) return;
            BtnDonHangBan.BorderWidth = 0;
            BtnDonHangMua.BorderWidth = 1;
            viewModel.OrderType = 0;
            viewModel.RefreshCommand.Execute(null);
        }
        private void DonHangBan_Clicked(object sender, EventArgs e)
        {
            if (viewModel.OrderType == 1) return;
            BtnDonHangBan.BorderWidth = 1;
            BtnDonHangMua.BorderWidth = 0;
            viewModel.OrderType = 1;
            viewModel.RefreshCommand.Execute(null);
        }

        // click button load by status
        private void LoadByStatus_Clicked(object sender, EventArgs e)
        {
            // clear border color of all button
            foreach (var item in StackLayoutButtonStatusGroup.Children)
            {
                ((RadButton)item).BorderThickness = 0;
            }
            // set border color for current button.
            ((RadButton)sender).BorderThickness = new Thickness(0, 0, 0, 3);
            // set status viewmodel from command paramater
            var btn = sender as Button;
            var commandParams = btn.CommandParameter;
            if (commandParams != null)
            {
                viewModel.Status = int.Parse(commandParams.ToString());
            }
            else
            {
                viewModel.Status = null;
            }
            viewModel.RefreshCommand.Execute(null);
        }
    }
}
