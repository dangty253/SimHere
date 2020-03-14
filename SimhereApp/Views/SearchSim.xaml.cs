using SimHere.Entities;
using SimhereApp.Portable.Controls;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.XamarinForms.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchSim : ContentPage
    {
        private SearchSimViewModel viewModel;

        private List<short> CarrierList;
        private List<int> NumberTypeList;
        private short? Menh;

        public SearchSim()
        {
            Initialized();
        }

        public void Initialized()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new SearchSimViewModel();
            CarrierList = new List<short>();
            NumberTypeList = new List<int>();

            btnClearPrefixNumber.IsVisible = false;

            /// set color  overlay cho các nhà mang 
            foreach (View item in gridCarrierOptions.Children)
            {
                StackLayout stackLayout = (StackLayout)((Grid)item).Children[1];
                stackLayout.BackgroundColor = Color.FromRgba(250, 250, 250, 0.4);
            }
            Init();
        }

        public async void Init()
        {
            await viewModel.LoadNumberTypes();
            int row = 0;
            for (int i = 0; i < 27; i += 2)
            {
                Grid grid = new Grid() { Padding = new Thickness(5) };
                BsdCheckbox left = new BsdCheckbox()
                {
                    Title = viewModel.NumberTypeOptions[i].Name,
                    Spacing = 7,
                    TitleColor = Color.FromHex("#444444"),
                    CheckedColor = Color.DarkGreen,
                };
                left.GestureRecognizers.Add(new TapGestureRecognizer()
                {
                    NumberOfTapsRequired = 1,
                    CommandParameter = i.ToString()
                });

                left.changeChecked += OnNumberTypeChecked;

                Grid.SetColumn(left, 0);
                grid.Children.Add(left);

                if (i < 26)
                {
                    int rightIndex = i + 1;
                    BsdCheckbox right = new BsdCheckbox()
                    {
                        Title = viewModel.NumberTypeOptions[rightIndex].Name,
                        Spacing = 7,
                        TitleColor = Color.FromHex("#444444"),
                        CheckedColor = Color.DarkGreen,
                    };
                    right.GestureRecognizers.Add(new TapGestureRecognizer()
                    {
                        NumberOfTapsRequired = 1,
                        CommandParameter = rightIndex.ToString()
                    });

                    right.changeChecked += OnNumberTypeChecked;
                    Grid.SetColumn(right, 1);
                    grid.Children.Add(right);
                }
                gridNumberTypes.Children.Add(grid);

                row++;
            }
            gridLoading.IsVisible = false;
        }

        // khi chon hoac bo chon number type.
        private async void OnNumberTypeChecked(object sender, EventArgs e)
        {
            var cb = sender as BsdCheckbox;
            var numberTypeIndex = int.Parse((cb.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter.ToString());
            var numberType = this.viewModel.NumberTypeOptions[numberTypeIndex];
            // neu check thi add vao list.
            if (cb.IsChecked)
            {
                NumberTypeList.Add(numberType.Id);
            }
            else // nguoc lai bo ra khoi list.
            {
                if (NumberTypeList.Any(x => x == numberType.Id))
                {
                    var test = NumberTypeList.Where(x => x == numberType.Id);
                    NumberTypeList.Remove(NumberTypeList.Single(x => x == numberType.Id));
                }
            }
        }

        private async void BtnSeach_Clicked(object sender, EventArgs e)
        {

            var model = new FilterModel();
            model.PriceFrom = priceFrom.Text;
            model.PriceTo = priceTo.Text;
            model.Keyword = EntryKeyword.Text;
            if (CarrierList.Any())
            {
                model.CarrierList = CarrierList;
            }

            List<int> ListNotInNumber = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                var checkbox = gridListNotInNumber.Children[i] as BsdCheckbox;
                if (checkbox.IsChecked)
                {
                    int num = int.Parse(checkbox.Title);
                    ListNotInNumber.Add(num);
                }
            }

            // co chon menh.
            if (this.Menh.HasValue)
            {
                int[] SoKhac = null;
                switch (this.Menh)
                {
                    case 0: // kim 
                        SoKhac = new int[] { 9 };
                        break;
                    case 1: // moc
                        SoKhac = new int[] { 6, 7 };
                        break;
                    case 2: // thuy
                        SoKhac = new int[] { 2, 5, 8 };
                        break;
                    case 3: // hoa
                        SoKhac = new int[] { 1 };
                        break;
                    case 4: // tho
                        SoKhac = new int[] { 3, 4 };
                        break;
                    default:
                        break;
                }

                if (SoKhac != null)
                {
                    for (int i = 0; i < SoKhac.Count(); i++)
                    {
                        if (ListNotInNumber.Any(x => x == SoKhac[i]) == false) // chua co trong list so ko bao gom
                        {
                            ListNotInNumber.Add(SoKhac[i]);
                        }
                    }
                }
            }


            if (ListNotInNumber.Any())
            {
                model.NotInNumber = Array.ConvertAll(ListNotInNumber.ToArray(), value => new int?(value));
            }



            if (PickerPrefixNumber.SelectedItem != null)
            {
                model.PrefixNumberId = ((PrefixNumber)PickerPrefixNumber.SelectedItem).Id;
            }

            if (NumberTypeList.Any())
            {
                model.NumberTypeList = NumberTypeList;
            }

            if (!string.IsNullOrWhiteSpace(EntryTotalPoint.Text))
            {
                model.TotalPoint = int.Parse(EntryTotalPoint.Text.Trim());
            }

            if (!string.IsNullOrWhiteSpace(EntryLastNumber.Text))
            {
                model.LastNumber = int.Parse(EntryLastNumber.Text.Trim());
            }

            await Navigation.PushAsync(new SearchResult(model));
        }

        private void XemHuongDanTimKiem_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Hướng dẫn tìm kiếm", @"- Sử dụng dấu x đại diện cho 1 số và dấu * đại diện cho một chuối số. " + Environment.NewLine +
                @"Để tìm sim bắt đầu bằng 098, quy khách nhập vào 098* " + Environment.NewLine +
                @"Để tìm sim kết thúc bằng 888, quý khách nhập vào *888 " + Environment.NewLine +
                @"Để tìm sim kết thúc bằng 888, quý khách nhập vào *888 " + Environment.NewLine +
                @"Để tìm sim bắt đầu bằng 098 và kết thúc 888, nhập vào 098*888, quý khách nhập vào *888 " + Environment.NewLine +
                @"Để tìm sim bên trong có số 888, nhập vào 888" + Environment.NewLine +
                @"Để tìm sim bắt đầu bằng 098 và kết thúc bằng 808, 818, 812, 813, 814, 815, 816, 817, 818, 819 nhập vào 098*8*8 " + Environment.NewLine, "Đóng");
        }

        private void SelectCarrier_Clicked(object sender, EventArgs e)
        {
            var grid = (Grid)sender;
            var tapGes = grid.GestureRecognizers[0] as TapGestureRecognizer;
            short carrierID = short.Parse(tapGes.CommandParameter.ToString());

            var stacklayout = ((Grid)sender).Children[1];
            var currentVisible = stacklayout.IsVisible;
            // dang chon => bo chon.
            // chua chon => chon.
            if (currentVisible)
            {
                stacklayout.IsVisible = false;
                // neu hien tai dang co. => xoadi.
                if (CarrierList.Any(x => x == carrierID))
                {
                    CarrierList.Remove(CarrierList.Single(x => x == carrierID));
                }
            }
            else
            {
                stacklayout.IsVisible = true;
                // chua co thi ad vao
                if (!CarrierList.Any(x => x == carrierID))
                {
                    CarrierList.Add(carrierID);
                }
            }
        }

        private async void Menh_Checked(object sender, EventArgs e)
        {
            foreach (RadBorder item in gridMenh.Children)
            {
                item.BorderColor = Color.Transparent;
                (item.Content as Label).FontAttributes = FontAttributes.None;
            }
            RadBorder radBorder = (RadBorder)sender;
            radBorder.BorderColor = Color.Black;
            (radBorder.Content as Label).FontAttributes = FontAttributes.Bold;
            this.Menh = short.Parse((radBorder.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter.ToString());
        }

        private void ClearPrefixNumber_Clicked(object sender, EventArgs e)
        {
            PickerPrefixNumber.SelectedItem = null;
        }

        private async void ResetSearchForm_Clicked(object sender, EventArgs e)
        {
            PickerPrefixNumber.SelectedItem = null;
            EntryKeyword.Text = "";
            EntryTotalPoint.Text = "";
            EntryLastNumber.Text = "";

            for (int i = 0; i < 9; i++)
            {
                var checkbox = gridListNotInNumber.Children[i] as BsdCheckbox;
                checkbox.IsChecked = false;
            }

            priceFrom.Text = null;
            priceTo.Text = null;

            // clear nha mang.
            this.CarrierList.Clear();
            foreach (var item in gridCarrierOptions.Children)
            {
                StackLayout stackLayout = (StackLayout)((Grid)item).Children[1];
                stackLayout.IsVisible = false;
            }

            // clear loai sim.
            this.NumberTypeList.Clear();
            foreach (Grid grid in gridNumberTypes.Children)
            {
                // 1 grid gom 1 hoac 2 checkbox.
                foreach (BsdCheckbox cb in grid.Children)
                {
                    cb.IsChecked = false;
                }
            }

            // clear menh
            this.Menh = null;
            if (gridMenh.Children.Any(x => x is RadBorder && ((RadBorder)x).BorderColor == Color.Black))
            {
                var rad = (RadBorder)gridMenh.Children.Single(x => x is RadBorder && ((RadBorder)x).BorderColor == Color.Black);
                rad.BorderColor = Color.Transparent;
            }
        }

        void PrefixNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnClearPrefixNumber.IsVisible = (((Picker)sender).SelectedIndex != -1);
        }
    }
}