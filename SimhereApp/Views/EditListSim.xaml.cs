using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class EditListSim : ContentPage
    {
        private readonly EditSimListViewModel viewModel;
        private List<string> ModifiedSimId = new List<string>();
        public EditListSimModel CurrentSim;
        public EditListSim()
        {
            InitializeComponent();
            CVNumberTypeOptions.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            SimListView.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                var Sim = e.Item as EditListSimModel;
                if (Sim.Id == viewModel.Data.LastOrDefault().Id)
                    await viewModel.LoadMoreData();
            };
            this.BindingContext = viewModel = new EditSimListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            if (viewModel.Data.Any() == false)
            {
                await DisplayAlert("", "Không có sim nào đang ở trạng thái chưa bán", "Quay lại");
                await Navigation.PopAsync();
            }
            await viewModel.LoadNumberTypes();
            gridLoading.IsVisible = false;
        }

        public async void Save_Clicked(object sender, EventArgs e)
        {
            gridLoading.IsVisible = true;
            var data = this.viewModel.Data.Where(x => x.IsModified && !string.IsNullOrWhiteSpace(x.DisplayNumber) && x.Price.HasValue).Select(x => new
            {
                Id = x.Id,
                DisplayNumber = x.DisplayNumber,
                Price = x.Price,
                CarrierId = x.CarrierId,
                Sim_NumberTypes = x.Sim_NumberTypes
            });
            if (data.Any() == false)
            {
                gridLoading.IsVisible = false;
                await DisplayAlert("", "Chỉnh sửa thông tin sim để cập nhật", "Đóng");
                return;
            }

            ApiResponse response = await ApiHelper.Put("api/sim/bulkupdate", data);
            gridLoading.IsVisible = false;
            if (response.IsSuccess)
            {
                await DisplayAlert("", "Cập nhật thành công", "Đóng");
            }
            else
            {
                await DisplayAlert("", "Lỗi, không thể cập nhật sim." + Environment.NewLine + response.Message, "Đóng");
            }
        }
        public async void OpenCarrierOption_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Chọn nhà mạng", "Huỷ", null, viewModel.CarrierOptions.Select(x => x.Name).ToArray());

            if (this.viewModel.CarrierOptions.Any(x => x.Name == action))
            {
                var selected = this.viewModel.CarrierOptions.SingleOrDefault(x => x.Name == action);

                StackLayout stackLayout = (StackLayout)sender;
                TapGestureRecognizer tapGestureRecognizer = stackLayout.GestureRecognizers[0] as TapGestureRecognizer;

                var selectedSim = tapGestureRecognizer.CommandParameter as EditListSimModel;
                selectedSim.CarrierId = selected.Id;
            }
        }


        public async void OpenNumberTypeOptions_Clicked(object sender, EventArgs e)
        {
            StackLayout stackLayout = (StackLayout)sender;
            TapGestureRecognizer tapGestureRecognizer = stackLayout.GestureRecognizers[0] as TapGestureRecognizer;
            this.CurrentSim = tapGestureRecognizer.CommandParameter as EditListSimModel;


            foreach (var item in CurrentSim.Sim_NumberTypes)
            {
                var type = viewModel.NumberTypeOptions.SingleOrDefault(x => x.Id == item.NumberTypeId);
                if (type != null)
                {
                    type.IsSelected = true;
                }
            }
            lvNumberType.ScrollTo(viewModel.NumberTypeOptions[0], ScrollToPosition.Start, false);
            CVNumberTypeOptions.IsVisible = true;
        }

        public async void SelectedNumberType_Clicked(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as SimTypeOption;
            item.IsSelected = !item.IsSelected;
        }

        public async void BtnCompleteSimType_Clicked(object sender, EventArgs e)
        {
            var selectedNumberTypes = viewModel.NumberTypeOptions.Where(x => x.IsSelected);
            var newNumberTypes = new List<Sim_NumberType>();
            foreach (var item in selectedNumberTypes)
            {
                newNumberTypes.Add(new Sim_NumberType()
                {
                    SimId = CurrentSim.Id,
                    NumberTypeId = item.Id,
                    NumberType = new NumberType()
                    {
                        Name = item.Name,
                    }
                });
            }


            this.CurrentSim.Sim_NumberTypes = newNumberTypes;
            CloseModalNumberTypes_Clicked(null, EventArgs.Empty);
        }

        public async void CloseModalNumberTypes_Clicked(object sender, EventArgs e)
        {
            CVNumberTypeOptions.IsVisible = false;
            UnCheckNumberTypes();

        }

        public async void DeleteNumberType_Clicked(object sender, EventArgs e)
        {
            UnCheckNumberTypes();
            this.CurrentSim.Sim_NumberTypes = new List<Sim_NumberType>();
            BtnCompleteSimType_Clicked(null, EventArgs.Empty);
        }


        public void UnCheckNumberTypes()
        {
            foreach (var item in viewModel.NumberTypeOptions)
            {
                item.IsSelected = false;
            }
        }
    }
}
