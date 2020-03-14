using SimhereApp.Portable.Settings;
using System.Windows.Input;
using Xamarin.Forms;
using SimHere.Entities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Views;
using System.Linq;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;

namespace SimhereApp.Portable.ViewModels
{
    public class MySimListViewModel : ListViewPageViewModel<Sim>
    {
        public bool IsLogged
        {
            get => UserLogged.IsLogged;
        }
        private int? _status;
        public int? Status
        {
            get => _status;
            set
            {
                _status = value;
                SearchValue = string.Empty;
                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(ShowEditButton));
            }
        }

        public bool ShowEditButton { get => _status == 0; }
        public ICommand LoadByStatusCommand
        {
            get
            {
                return new Command<string>((status) =>
                {
                    if (status == null)
                    {
                        this.Status = null;
                    }
                    else
                    {
                        this.Status = int.Parse(status);
                    }
                    ReLoadDataCommand();
                });
            }
        }
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }
        public MySimListViewModel()
        {
            if (UserLogged.IsLogged)
            {
                Filter = new FilterModel();
                Filter.OwnerId = UserLogged.Id;
                PreLoadData = new Command(() =>
                {
                    Filter.Status = Status;
                    Filter.Keyword = SearchValue;
                    string jsonStringFilterModel = JsonConvert.SerializeObject(Filter);
                    ApiUrl = $"api/sim/filter?Page={Page}&filterModel={jsonStringFilterModel}";
                });
                MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel{ Name = "Thêm Sim mới", IconString= "\uf067", Command = AddNewSimsCommand},
                new MenuItemModel{ Name = "Đăng bán tất cả", IconString= "\uf7c4", Command = PublicAllSimsCommand},
                new MenuItemModel{ Name = "Sửa nhanh", IconString= "\uf044", Command = BulkdEditSimsCommand},
                new MenuItemModel{ Name = "Tải lại trang", IconString= "\uf2f9", Command = RefreshCommand},
            };
            }

        }


        public Command AddNewSimsCommand => new Command(async () =>
        {
            var actionSheet = await Shell.Current.DisplayActionSheet("Thêm sim", "Huỷ", null, "Thêm", "Import từ Text");
            if (actionSheet == "Thêm")
            {
                if (string.IsNullOrEmpty(UserLogged.Phone))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng cập nhật số điện thoại trước khi tạo sim", "Đóng");
                    return;
                }
                await Shell.Current.Navigation.PushAsync(new PostSim());
            }
            else if (actionSheet == "Import từ Text")
            {
                if (string.IsNullOrEmpty(UserLogged.Phone))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng cập nhật số điện thoại trước khi tạo sim", "Đóng");
                    return;
                }
                await Shell.Current.Navigation.PushAsync(new AddSimByText());
            }
        });
        public Command PublicAllSimsCommand => new Command(async () =>
        {
            var confirm = await Shell.Current.DisplayAlert("Xác nhận", "Bạn có muốn đăng bán tất cả sim không", "Đồng ý", "Đóng");
            if (!confirm) return;
            ApiResponse response = await ApiHelper.Put("api/sim/buyall", null);
            if (response.IsSuccess)
            {
                RefreshCommand.Execute(null);
                await Shell.Current.DisplayAlert("", response.Message, "Đóng");
            }
            else
            {
                await Shell.Current.DisplayAlert("", response.Message, "Đóng");
            }
        });
        public Command BulkdEditSimsCommand => new Command(async () =>
        {
            if ((Status.HasValue == false || Status == 0) && !Data.Any())
            {
                await Shell.Current.DisplayAlert("", "Không có sim chưa bán để sửa", "Đóng");
            }
            else
            {
                await Shell.Current.Navigation.PushAsync(new EditListSim());
            }
        });
    }
}
