using SimHere.Entities;
using SimHere.Entities.Response;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PostSim : ContentPage
    {
        private List<Int16> ListCurrentSelectedNumberType;
        private bool NumberTypeChanged = false;

        private readonly PostSimViewModel viewModel;
        private string SimId;
        public PostSim()
        {
            InitializeComponent();
            Title = "Tạo sim mới";
            this.BindingContext = viewModel = new PostSimViewModel();
            InitAdd().GetAwaiter();
            BtnPost.IsVisible = true;
        }

        public async Task InitAdd()
        {
            await viewModel.LoadNumberTypes();
            viewModel.SelectedImage = viewModel.Images[0];
            MainGrid.IsVisible = true;
            gridLoading.IsVisible = false;
        }

        public PostSim(string Id)
        {
            InitializeComponent();
            SimId = Id;
            this.BindingContext = viewModel = new PostSimViewModel();
            InitUpdate().GetAwaiter();
        }

        public void AddDeleteToolbar()
        {
            Xamarin.Forms.ToolbarItem deleteToolBar = new Xamarin.Forms.ToolbarItem();
            deleteToolBar.Text = "Xóa";
            deleteToolBar.Priority = 0;
            deleteToolBar.Order = ToolbarItemOrder.Default;
            deleteToolBar.Clicked += DeleteToolbar_Clicked;
            this.ToolbarItems.Add(deleteToolBar);
        }

        public async Task InitUpdate()
        {
            labelStatus.IsVisible = true;
            entryStatus.IsVisible = true;
            entryNumber.IsEnabled = false;
            viewModel.IsLoading = false;
            await viewModel.LoadNumberTypes();
            var result = await ApiHelper.Get<Sim>("api/sim/" + SimId);
            if (result.IsSuccess)
            {
                var sim = (Sim)result.Content;
                Title = sim.DisplayNumber;

                var model = new AddSimViewModel()
                {
                    SimNumber = sim.SimNumber,
                    DisplayNumber = sim.DisplayNumber,
                    Price = sim.Price,
                    Carrier = viewModel.CarrierOptions.SingleOrDefault(x => x.Id == sim.CarrierId),
                    Status = sim.Status.Value,
                    StatusFormat = sim.StatusFormat,
                    Description = sim.Description
                };

                if (sim.ImageId.HasValue)
                {
                    viewModel.SelectedImage = viewModel.Images.SingleOrDefault(x => x.Id == sim.ImageId.Value);
                }

                if (sim.Status == 0) // chua ban
                {
                    BtnDangSim.IsVisible = true;
                    BtnPost.IsVisible = true;
                    AddDeleteToolbar();
                }
                else if (sim.Status == 1) // dang mo ban
                {
                    BtnCancle.IsVisible = true;
                }
                else if (sim.Status == 2) // da ban
                {

                }
                else if (sim.Status == 3) // huy
                {

                }
                else if (sim.Status == 4)
                {

                }

                viewModel.Sim = model;

                if (sim.SubcribeTypeId.HasValue)
                {
                    viewModel.Sim.SubcribeType = viewModel.SubcribeTypeOptions.SingleOrDefault(x => x.Id == sim.SubcribeTypeId.Value);
                }

                foreach (var item in viewModel.NumberTypeOptions)
                {
                    if (sim.Sim_NumberTypes.Any(x => x.NumberTypeId == item.Id))
                    {
                        item.IsSelected = true;
                    }
                }

                InitButtonGroup();
                BtnCompleteSimType_Clicked(null, null);
            }
            MainGrid.IsVisible = true;
            gridLoading.IsVisible = false;
        }

        private async void BtnPost_Clicked(object sender, EventArgs e)
        {

            AddSimViewModel model = viewModel.Sim;
            if (string.IsNullOrWhiteSpace(model.SimNumber))
            {
                await DisplayAlert("", "Vui lòng nhập số sim", "Đóng");
                return;
            }

            if (model.SimNumber.Length < 9)
            {
                await DisplayAlert("", "Số Sim không đúng định dạng", "Đóng");
                return;
            }

            if (string.IsNullOrWhiteSpace(model.DisplayNumber))
            {
                await DisplayAlert("", "Vui lòng nhập số sim", "Đóng");
                return;
            }

            if (model.Carrier == null)
            {
                await DisplayAlert("", "Vui lòng chọn nhà mạng", "Đóng");
                return;
            }

            if (model.Price.HasValue == false)
            {
                await DisplayAlert("", "Vui lòng nhập giá sim", "Đóng");
                return;
            }
            gridLoading.IsVisible = true;

            Sim sim = new Sim();
            sim.SimNumber = model.SimNumber;
            sim.DisplayNumber = model.DisplayNumber;
            sim.Price = model.Price.Value;
            sim.CarrierId = model.Carrier.Id;
            sim.Description = model.Description;
            if (model.SubcribeType != null) sim.SubcribeTypeId = model.SubcribeType.Id;

            if (SimId != null) // sửa thì gán thêm id.
            {
                sim.Id = SimId;
            }

            IEnumerable<NumberType> numberTypes = viewModel.NumberTypeOptions.Where(x => x.IsSelected).Select(x => new NumberType()
            {
                Id = x.Id
            });

            AddUpdateSimViewModel addUpdateSimViewModel = new AddUpdateSimViewModel();
            addUpdateSimViewModel.Sim = sim;
            addUpdateSimViewModel.NumberTypes = numberTypes;

            ApiResponse response = null;

            if (SimId == null)
            {
                response = await ApiHelper.Post("api/sim", addUpdateSimViewModel, true);
            }
            else
            {
                response = await ApiHelper.Put("api/sim", addUpdateSimViewModel, true);
            }

            if (response.IsSuccess)
            {
                if (SimId == null)
                {
                    //MySimList mySimList = ((App.Current.MainPage as MainPage).Children[3] as NavigationPage).RootPage as MySimList;
                    //MySimListViewModel mySimListVM = mySimList.BindingContext as MySimListViewModel;

                    MySimListViewModel mySimListVM = Shell.Current.BindingContext as MySimListViewModel;

                    //mySimListVM.RefreshCommand.Execute(null);
                    //await Navigation.PopAsync(false);
                    await mySimListVM.LoadOnRefreshCommandAsync();
                    var added = mySimListVM.Data[0];

                    SimId = added.Id;

                    gridLoading.IsVisible = false;
                    var actionSheet = await DisplayActionSheet("Lưu thành công. Bạn có muốn đăng bán sim này không?", "Để sau", null, "Đăng bán");
                    if (actionSheet == "Đăng bán")
                    {
                        BtnDangSimClicked(sender, EventArgs.Empty);
                    }
                    else
                    {
                        await Shell.Current.Navigation.PopAsync();
                        await Shell.Current.Navigation.PushAsync(new PostSim(SimId), false);
                        XFToast.ShortMessage("Lưu thành công !");
                    }
                }
                else
                {

                    gridLoading.IsVisible = false;
                    XFToast.ShortMessage("Lưu thành công !");
                }
            }
            else
            {
                await DisplayAlert("", response.Message, "Đóng");
                gridLoading.IsVisible = false;
            }
        }

        private async void BtnDangSimClicked(object sender, EventArgs e)
        {
            gridLoading.IsVisible = true;
            await ApiHelper.Put("api/sim/status", new AddUpdateSimViewModel()
            {
                Sim = new Sim()
                {
                    Status = 1,
                    Id = SimId
                }
            }, true);

            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new PostSim(SimId), false);
            XFToast.ShortMessage("Đăng thành công !");
        }

        private async void BtnCancleClicked(object sender, EventArgs e)
        {
            gridLoading.IsVisible = true;
            await ApiHelper.Put("api/sim/status", new AddUpdateSimViewModel()
            {
                Sim = new Sim()
                {
                    Status = 0,
                    Id = SimId
                }
            }, true);
            await Shell.Current.Navigation.PopAsync();
            await Shell.Current.Navigation.PushAsync(new PostSim(SimId), false);
            XFToast.ShortMessage("Huỷ thành công !");
        }

        private void EntryLoaiSim_Focused(object sender, FocusEventArgs e)
        {
            // Khi mở modal lên thì lưu lại danh sách các option đã chọn
            // Khi có thay đổi (check,uncheck) thì đánh dấu là đã thay đổi. numbertypechange
            // khi nhấn nút xóa trên popup cũng cập nhật là đã thay đổi. numbertypechange
            // khi chọn xong thì cập nhật lại giao diện và xóa bỏ đánh dấu, xóa bỏ list backup.
            // khi đóng thì lấy list backup ra set giá trị. rồi cũng clear luôn.
            entryLoaiSim.Unfocus();
            BackUp_LoaiSim();
            modalContentView.IsVisible = true;
        }

        private void BackUp_LoaiSim()
        {
            // khơi tạo nếu chưa khởi tạo trước đó.
            if (ListCurrentSelectedNumberType == null)
            {
                ListCurrentSelectedNumberType = new List<Int16>();
            }

            // Dưa list id của numbertype hiện tại vào để backup
            viewModel.NumberTypeOptions.Where(x => x.IsSelected).ToList().ForEach(x => { ListCurrentSelectedNumberType.Add(x.Id); });
        }

        private void BtnCompleteSimType_Clicked(object sender, EventArgs e)
        {
            SimTypeSelected.Children.Clear();
            var selectedList = viewModel.NumberTypeOptions.Where(x => x.IsSelected).ToList();
            var selectedListCount = selectedList.Count();
            for (int i = 0; i < selectedListCount; i++)
            {
                var item = selectedList[i];

                StackLayout stackLayout = new StackLayout()
                {
                    BackgroundColor = Color.White,
                    Padding = new Thickness(10, 5, 10, 5),
                    Orientation = StackOrientation.Horizontal
                };

                Label label = new Label()
                {
                    Text = item.Name,
                    TextColor = Color.FromHex("#3F51B5"),
                    FontSize = 14
                };
                stackLayout.Children.Add(label);

                RadBorder radBorder = new RadBorder();
                radBorder.Margin = 2;
                radBorder.CornerRadius = 5;
                radBorder.BorderColor = Color.FromHex("#3F51B5");
                radBorder.Content = stackLayout;

                SimTypeSelected.Children.Add(radBorder);
            }



            if (SimTypeSelected.Children.Any())
            {
                entryLoaiSim.IsVisible = false;
                //Button button = new Button()
                //{
                //    BackgroundColor = Color.FromHex("#3F51B5"),
                //    HeightRequest = 30,
                //    //Padding = 5,
                //    Text="Thêm",
                //    FontSize = 10,
                //    //Image = "plus_white.png"
                //};
                //button.Clicked += FlexSimTypeTapGes_Tapped;
                //SimTypeSelected.Children.Add(button);
            }
            else
            {
                entryLoaiSim.IsVisible = true;
            }

            ClearBackUpNumberType();
            modalContentView.IsVisible = false;
        }

        private async void FlexSimTypeTapGes_Tapped(object sender, EventArgs e)
        {
            viewModel.IsLoading = true;
            BackUp_LoaiSim();
            modalContentView.IsVisible = true;
            viewModel.IsLoading = false;
        }

        private void BtnClearSimType_Clicked(object sender, EventArgs e)
        {
            var selectedList = viewModel.NumberTypeOptions.Where(x => x.IsSelected);
            if (selectedList.Any())
            {
                foreach (var item in selectedList)
                {
                    item.IsSelected = false;
                }
                NumberTypeChanged = true;
            }
        }

        private void BtnCloseNumberTypeModal_Clicked(object sender, EventArgs e)
        {
            if (NumberTypeChanged) // có thay đổi 
            {
                if (ListCurrentSelectedNumberType != null && ListCurrentSelectedNumberType.Any())
                {
                    foreach (var item in viewModel.NumberTypeOptions)
                    {
                        // có trong list backup
                        if (ListCurrentSelectedNumberType.Any(x => x == item.Id))
                        {
                            item.IsSelected = true;
                        }
                        else
                        {
                            item.IsSelected = false;
                        }
                    }
                    ClearBackUpNumberType();
                }
            }
            modalContentView.IsVisible = false;
        }

        private void ClearBackUpNumberType()
        {
            // clear backup number type la chưa thay đổi, và clear trong list backup
            NumberTypeChanged = false;
            if (ListCurrentSelectedNumberType != null && ListCurrentSelectedNumberType.Any())
            {
                ListCurrentSelectedNumberType?.Clear();
            }

        }

        private void ListViewNumerTypeOptions_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            NumberTypeChanged = true; // đánh dấu thay đổi
            var item = e.Item as SimTypeOption;
            item.IsSelected = !item.IsSelected;
        }

        private async void DeleteToolbar_Clicked(object sender, EventArgs e)
        {
            var confirm = await DisplayAlert("Xác nhận", "Bạn có muốn xóa sim này không", "Đồng ý", "Đóng");
            if (!confirm) return;

            gridLoading.IsVisible = true;
            List<string> ListSimSelected = new List<string>() { SimId };
            ApiResponse response = await ApiHelper.Delete("api/sim?Ids=" + string.Join(",", ListSimSelected.ToArray()));
            if (response.IsSuccess)
            {
                await Shell.Current.Navigation.PopAsync();
                MessagingCenter.Send<PostSim, string>(this, "DeleteSim", SimId);
            }
            else
            {
                await DisplayAlert("", response.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }


        private void InitButtonGroup()
        {
            gridBtnGroup.ColumnDefinitions.Clear();
            var buttons = gridBtnGroup.Children.Where(x => x is RadButton && x.IsVisible == true).ToList();
            var count = buttons.Count();
            for (int i = 0; i < count; i++)
            {
                gridBtnGroup.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star),
                });
                Grid.SetColumn(buttons[i], i);
            }
        }
    }
}