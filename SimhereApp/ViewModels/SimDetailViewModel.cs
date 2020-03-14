using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class SimDetailViewModel : BaseViewModel
    {
        public ObservableCollection<Carrier> CarrierOptions { get; set; }
        public ObservableCollection<SubcribeType> SubcribeTypeOptions { get; set; }
        public ObservableCollection<SimTypeOption> NumberTypeOptions { get; set; }
        private SimDetailModel _sim;
        public SimDetailModel Sim
        {
            get => _sim;
            set
            {
                if (_sim != value)
                {
                    _sim = value;
                    OnPropertyChanged(nameof(Sim));
                    OnPropertyChanged(nameof(HasDetail));
                }
            }
        }
        private bool _isFollow;
        public bool IsFollow
        {
            get => _isFollow;
            set
            {
                if (_isFollow != value)
                {
                    _isFollow = value;
                    OnPropertyChanged(nameof(IsFollow));
                }
            }
        }
        public bool HasDetail
        {
            get => this.Sim != null ? this.Sim.Description != null : false;
        }
        private string _numberTypesFormat;
        public string NumberTypesFormat
        {
            get => _numberTypesFormat;
            set
            {
                if (_numberTypesFormat != value)
                {
                    _numberTypesFormat = value;
                    OnPropertyChanged(nameof(NumberTypesFormat));
                }
            }
        }
        private string SimId;
        public SimDetailViewModel(string id)
        {
            CarrierOptions = new ObservableCollection<Carrier>(CarrierData.Get());
            SubcribeTypeOptions = new ObservableCollection<SubcribeType>(SubcribeTypeData.Get());
            NumberTypeOptions = new ObservableCollection<SimTypeOption>();
            SimId = id;
            Sim = new SimDetailModel();
        }
        public async Task LoadNumberTypes()
        {
            var result = await ApiHelper.Get<List<NumberType>>("api/sim/numbertypes");
            if (result.IsSuccess)
            {
                var numberTypes = result.Content as List<NumberType>;
                var numberTypesCount = numberTypes.Count();
                for (int i = 0; i < numberTypesCount; i++)
                {
                    var numberType = numberTypes[i];

                    NumberTypeOptions.Add(new SimTypeOption(numberType.Name)
                    {
                        Id = numberType.Id
                    });
                }
            }
            else
            {
                throw new Exception("không tim thấy dữ liệu");
            }
        }
        public async Task GetUserFollowSimStatus()
        {
            ApiResponse response = await ApiHelper.Get<object>("api/sim/isfollow/" + SimId, true);
            if (response.IsSuccess)
            {
                IsFollow = (bool)response.Content;
            }
            else
            {
                IsFollow = false;
            }
        }
        public ICommand CallCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!UserLogged.IsLogged)
                    {
                        await Shell.Current.DisplayAlert("", "Vui lòng đăng nhập để thực hiện chức năng này", "Đăng nhập");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                    else
                    {
                        PlacePhoneCall(Sim.Owner.PhoneNumber);
                    }
                });
            }
        }
        public ICommand SendMessageCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!UserLogged.IsLogged)
                    {
                        await Shell.Current.DisplayAlert("", "Vui lòng đăng nhập để thực hiện chức năng này", "Đăng nhập");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                    else
                    {
                        string action = await Shell.Current.DisplayActionSheet("Bạn muốn nhắn tin qua số điện thoại hay chat ?", "Huỷ", null, "SMS", "Chat");
                        if (action == "Chat")
                        {
                            //namvlchat
                            //await Shell.Current.Navigation.PushAsync(new ChatDetail(Sim.Owner));
                            await Shell.Current.Navigation.PushAsync(new ChatMessagePage(new UserLite { Id = Sim.Owner.Id, FullName = Sim.Owner.FullName, PictureUrl = Sim.Owner.PictureUrl }));
                        }
                        else if (action == "SMS")
                        {
                            await SendSms($"Xin chào {Sim.Owner.FullName}.{Environment.NewLine}Tôi đang quan tâm đến sim số {Sim.DisplayNumber} của bạn.", Sim.Owner.PhoneNumber);
                        }
                    }
                });
            }
        }
        public ICommand OrderCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (UserLogged.IsLogged)
                    {
                        await Shell.Current.Navigation.PushAsync(new Views.OrderPage(Sim.Id, Sim.Price));
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                });
            }
        }
        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền hoặc không hỗ trợ chức năng này.  ", "Đóng");
            }
        }
        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền hoặc không hỗ trợ chức năng này.  ", "Đóng");
                // Other error has occurred.
            }
        }
    }
}
