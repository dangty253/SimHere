using Plugin.Messaging;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Configuration;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimDetail2 : ContentPage
    {
        private SimDetailViewModel viewModel;
        private readonly string SimId;
        public decimal? FixedPrice { get; set; } 
        public SimDetail2(string simId, decimal? price = null)
        {
            InitializeComponent();
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                this.Content = new NotConnectContentView();
                return;
            }

            SimId = simId;
            GridTitle.BackgroundColor = Color.FromRgba(250, 250, 250, 0.8);
            gridLoading.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
            this.BindingContext = viewModel = new SimDetailViewModel(SimId);
            FixedPrice = null;
            if (price != null)
            {
                FixedPrice = price;
            }
            Init().GetAwaiter();
            
        }
        public async Task Init()
        {
            var tasks = new Task[]{
                FetchSim(),
                viewModel.GetUserFollowSimStatus()
            };
           
            await Task.WhenAll(tasks);
            lblPrice.SetBinding(Label.TextProperty, new Binding("Sim.Price", stringFormat: "{0:0,0đ}"));

            // kiem tra neu user xem sim nay la chu sim. thi an cac nut goi, chat, mua.
            if (UserLogged.IsLogged && UserLogged.Id == viewModel.Sim.OwnerId)
            {
                gridBtn.IsVisible = false;
                BtnFollow.IsVisible = false;
                BtnUnFollow.IsVisible = false;
            }
            else
            {
                gridBtn.IsVisible = true;
            }

            MainScrollView.IsVisible = true;

            // an loading.
            gridLoading.IsVisible = false;

        }
        public async Task FetchSim()
        {
            var result = await ApiHelper.Get<Sim>("api/sim/" + SimId);
            if (result.IsSuccess)
            {
                var sim = (Sim)result.Content;
                Title = sim.DisplayNumber;

                var model = new SimDetailModel();
                model.Id = sim.Id;
                model.SimNumber = sim.SimNumber;
                model.DisplayNumber = sim.DisplayNumber;
                model.Price = sim.Price;
                if (FixedPrice != null)
                {
                    model.Price = (decimal)FixedPrice;
                }
                model.Carrier = viewModel.CarrierOptions.SingleOrDefault(x => x.Id == sim.CarrierId);
                model.OwnerId = sim.OwnerId;
                model.Owner = sim.Owner;
                model.CreatedOn = sim.CreatedOn;
                model.Status = sim.Status.Value;
                model.StatusFormat = sim.StatusFormat;
                model.Description = sim.Description;

                // ko co ida chi thi an stack layout dia chi di.
                if (string.IsNullOrEmpty(model.Owner.Address))
                {
                    address.IsVisible = false;
                }
                else
                {
                    address.IsVisible = true;
                }

                viewModel.Sim = model;
                if (sim.SubcribeTypeId.HasValue)
                {
                    viewModel.Sim.SubcribeType = viewModel.SubcribeTypeOptions.SingleOrDefault(x => x.Id == sim.SubcribeTypeId.Value);
                }

                // ngay sinh, than tai.
                List<string> NumberTypeStringList = new List<string>();

                await viewModel.LoadNumberTypes();
                foreach (var item in viewModel.NumberTypeOptions)
                {
                    if (sim.Sim_NumberTypes.Any(x => x.NumberTypeId == item.Id))
                    {
                        NumberTypeStringList.Add(item.Name);
                    }
                }
                viewModel.NumberTypesFormat = string.Join(", ", NumberTypeStringList.ToArray());

                // co loai sim thi lay id cua loai sim de lay background.
                if (sim.Sim_NumberTypes.Count > 0)
                {
                    short numberTypeId = sim.Sim_NumberTypes.FirstOrDefault().NumberTypeId;

                    string imageName = Constants.SimDetailBackgroundDictionary.keys[numberTypeId];
                    if (imageName == "")
                    {
                        SimImage.Source = "sim1.jpg";
                    }
                    else
                    {
                        SimImage.Source = AppConfig.API_IP + "HinhLoaiSim/" + imageName;
                    }
                }

                if (!string.IsNullOrEmpty(sim.Owner.PictureUrl))
                {
                    img_avatar.Source = AvatarHelper.ToAvatarUrl(sim.Owner.PictureUrl);
                }
                else
                {
                    var avatarText = AvatarHelper.NameToAvatarText(viewModel.Sim.Owner.FullName);
                    img_avatar.Source = ImageSource.FromUri(new Uri(AvatarHelper.PlaceholderLink(avatarText)));
                }
            }
        }
        private async void BtnFollow_Clicked(object sender, EventArgs e)
        {
            if (UserLogged.IsLogged == false)
            {
                await DisplayAlert("", "Vui lòng đăng nhập để thực hiện chức năng này", "Đăng nhập");
                await Shell.Current.GoToAsync("//homes/account");
            }
            else
            {
                gridLoading.IsVisible = true;
                if (viewModel.IsFollow)
                {
                    // bor theo doi
                    ApiResponse resonse = await ApiHelper.Post("api/sim/unfollowsim/" + SimId, null, true);
                    if (resonse.IsSuccess)
                    {
                        viewModel.IsFollow = false;
                    }
                }
                else
                {
                    // theo doi
                    ApiResponse resonse = await ApiHelper.Post("api/sim/followsim/" + SimId, null, true);
                    if (resonse.IsSuccess)
                    {
                        viewModel.IsFollow = true;
                    }
                }
                gridLoading.IsVisible = false;
            }
        }
        private async void ViewProfile_Clicked(object sender, EventArgs e)
        {
            if (viewModel.Sim.OwnerId == UserLogged.Id)
            {
                await Shell.Current.GoToAsync("//homes/account");
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new UserProfile(viewModel.Sim.OwnerId));
            }

        }
        void Handle_LayoutChanged(object sender, System.EventArgs e)
        {
            double originalWidth = 612;
            double originalHeight = 306;

            var layout = sender as StackLayout;

            double screenWidth = layout.Width;

            double rate = originalWidth / screenWidth;
            double layoutHeight = originalHeight / rate;

            stackImage.HeightRequest = layoutHeight;

            SimImage.WidthRequest = layout.Width;
            SimImage.HeightRequest = layoutHeight;
        }
    }
}