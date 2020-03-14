using SimHere.Entities;
using SimHere.Entities.Response;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        #region Attributes
        Order _OrderDetail;
        public Order OrderDetail
        {
            get => _OrderDetail;
            set
            {
                _OrderDetail = value;
                OnPropertyChanged(nameof(OrderDetail));
            }
        }
        bool _IsOwner;
        public bool IsOwner
        {
            get => _IsOwner;
            set
            {
                _IsOwner = value;
                if (!value && IsNewOrder)
                {
                    IsVisibleBuyerEditable = true;
                    IsVisibleBuyerInfo = false;
                }
                else
                {
                    IsVisibleBuyerEditable = false;
                    IsVisibleBuyerInfo = true;
                }
                OnPropertyChanged(nameof(IsOwner));
            }
        }
        bool _IsNewOrder;
        public bool IsNewOrder
        {
            get => _IsNewOrder;
            set
            {
                _IsNewOrder = value;
     
                OnPropertyChanged(nameof(IsNewOrder));
            }
        }
        Users _Owner;
        public Users Owner
        {
            get => _Owner;
            set
            {
                _Owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
        ProfileViewModel _Buyer;
        public ProfileViewModel Buyer
        {
            get => _Buyer;
            set
            {
                _Buyer = value;
                OnPropertyChanged(nameof(Buyer));
            }
        }
        Sim _SimDetail;
        public Sim SimDetail
        {
            get => _SimDetail;
            set
            {
                _SimDetail = value;
                OnPropertyChanged(nameof(SimDetail));
            }
        }
        bool _IsVisibleBuyerInfo;
        public bool IsVisibleBuyerInfo
        {
            get => _IsVisibleBuyerInfo;
            set
            {
                _IsVisibleBuyerInfo = value;
                OnPropertyChanged(nameof(IsVisibleBuyerInfo));
            }
        }
        bool _IsVisibleBuyerEditable;
        public bool IsVisibleBuyerEditable
        {
            get => _IsVisibleBuyerEditable;
            set
            {
                _IsVisibleBuyerEditable = value;
                OnPropertyChanged(nameof(IsVisibleBuyerEditable));
            }
        }
        #region
        //bool _btnBuyerContact;
        //public bool btnBuyerContact
        //{
        //    get => _btnBuyerContact;
        //    set
        //    {
        //        _btnBuyerContact = value;
        //        OnPropertyChanged(nameof(btnBuyerContact));
        //    }
        //}
        //bool _btnBuyerOrder;
        //public bool btnBuyerOrder
        //{
        //    get => _btnBuyerOrder;
        //    set
        //    {
        //        _btnBuyerOrder = value;
        //        OnPropertyChanged(nameof(btnBuyerOrder));
        //    }
        //}
        //bool _btnBuyerCancle;
        //public bool btnBuyerCancle
        //{
        //    get {
        //        return _btnBuyerCancle;
        //            }
        //    set
        //    {
        //        _btnBuyerCancle = value;
        //        OnPropertyChanged(nameof(btnBuyerCancle));
        //    }
        //}
        //bool _btnOwnerContact;
        //public bool btnOwnerContact
        //{
        //    get => _btnOwnerContact;
        //    set
        //    {
        //        _btnOwnerContact = value;
        //        OnPropertyChanged(nameof(btnOwnerContact));
        //    }
        //}
        //bool _btnOwnerConfirm;
        //public bool btnOwnerConfirm
        //{
        //    get => _btnOwnerConfirm;
        //    set
        //    {
        //        _btnOwnerConfirm = value;
        //        OnPropertyChanged(nameof(btnOwnerConfirm));
        //    }
        //}
        //bool _btnOwnerComplete;
        //public bool btnOwnerComplete
        //{
        //    get => _btnOwnerComplete;
        //    set
        //    {
        //        _btnOwnerComplete = value;
        //        OnPropertyChanged(nameof(btnOwnerComplete));
        //    }
        //}
        //bool _btnOwnerCancel;
        //public bool btnOwnerCancel
        //{
        //    get => _btnOwnerCancel;
        //    set
        //    {
        //        _btnOwnerCancel = value;
        //        OnPropertyChanged(nameof(btnOwnerCancel));
        //    }
        //}
        //int _OrderStatus;
        //public int OrderStatus
        //{
        //    set
        //    {
        //        if (value == -1)
        //        {
        //            btnBuyerContact = false;
        //            btnBuyerOrder = true;
        //            btnBuyerCancle = false;
        //        }
        //        else
        //        {
        //            if (value == 0)
        //            {
        //                if (IsOwner)
        //                {
        //                    btnOwnerContact = false;
        //                    btnOwnerConfirm = false;
        //                    btnOwnerComplete = false;
        //                    btnOwnerCancel = true;
        //                }
        //                else
        //                {
        //                    btnBuyerContact = false;
        //                    btnBuyerOrder = false;
        //                    btnBuyerCancle = true;
        //                }
        //            }
        //            else if (value == 1)
        //            {
        //                if (IsOwner)
        //                {
        //                    btnOwnerContact = false;
        //                    btnOwnerConfirm = false;
        //                    btnOwnerComplete = true;
        //                    btnOwnerCancel = false;
        //                }
        //                else
        //                {
        //                    btnBuyerContact = true;
        //                    btnBuyerOrder = false;
        //                    btnBuyerCancle = false;
        //                }
        //            } else if (value == 2 )
        //            {
        //                if (IsOwner)
        //                {
        //                    btnOwnerContact = false;
        //                    btnOwnerConfirm = false;
        //                    btnOwnerComplete = false;
        //                    btnOwnerCancel = false;
        //                }
        //                else
        //                {
        //                    btnBuyerContact = false;
        //                    btnBuyerOrder = false;
        //                    btnBuyerCancle = false;
        //                }
        //            }
        //            else if (value == 3 )
        //            {
        //                if (IsOwner)
        //                {
        //                    btnOwnerContact = true;
        //                    btnOwnerConfirm = false;
        //                    btnOwnerComplete = false;
        //                    btnOwnerCancel = false;
        //                }
        //                else
        //                {
        //                    btnBuyerContact = true;
        //                    btnBuyerOrder = false;
        //                    btnBuyerCancle = false;
        //                }
        //            }

        //        }
        //        _OrderStatus = value;
        //    }
        //}
        #endregion
        int _OrderStatus;
        public int OrderStatus
        {
            set
            {
                MenuItems = new ObservableCollection<MenuItemModel>();
                if (value == -1)
                {
                    MenuItems.Add(new MenuItemModel { Name = "Liên hệ Người bán", Command = ContactOrderCommand, IconString = "\uf2bb" });
                    MenuItems.Add(new MenuItemModel { Name = "Đặt hàng", Command = PlaceOrderCommand, IconString = "\uf058" });
                }
                else
                {
                    if (value == 0)
                    {
                        if (IsOwner)
                        {
                            MenuItems.Add(new MenuItemModel { Name = "Liên hệ người mua", Command = ContactOrderCommand, IconString = "\uf2bb" });
                            MenuItems.Add(new MenuItemModel { Name = "Xác nhận đơn hàng", Command = ConfirmOrderCommand, IconString = "\uf058" });
                            MenuItems.Add(new MenuItemModel { Name = "Hủy đơn hàng", Command = CancelOrderCommand, IconString = "\uf00d" });
                        }
                        else
                        {
                            MenuItems.Add(new MenuItemModel { Name = "Liên hệ Người bán", Command = ContactOrderCommand, IconString = "\uf2bb" });
                            MenuItems.Add(new MenuItemModel { Name = "Hủy đơn hàng", Command = CancelOrderCommand, IconString = "\uf00d" });
                        }
                    }
                    else if (value == 1)
                    {
                        if (IsOwner)
                        {
                            MenuItems.Add(new MenuItemModel { Name = "Liên hệ người mua", Command = ContactOrderCommand, IconString = "\uf2bb" });
                            MenuItems.Add(new MenuItemModel { Name = "Hoàn thành hàng", Command = CompleteOrderCommand, IconString = "\uf058" });
                        }
                        else
                        {
                            MenuItems.Add(new MenuItemModel { Name = "Liên hệ Người bán", Command = ContactOrderCommand, IconString = "\uf2bb" });
                        }
                    }
                    else if (value == 2 || value == 3)
                    {
                        if (IsOwner)
                        {
                            MenuItems.Add(new MenuItemModel { Name = "Liên hệ người mua", Command = ContactOrderCommand, IconString = "\uf2bb" });
                        }
                        else
                        {
                            MenuItems.Add(new MenuItemModel {  Name = "Liên hệ Người bán", Command = ContactOrderCommand, IconString = "\uf2bb" });
                        }
                    }

                }
                _OrderStatus = value;
                OnPropertyChanged("MenuItems");
            }
        }
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }
        #endregion

        #region Contructors
        public OrderViewModel() { }
        public OrderViewModel(string orderId)
        {
            Init(orderId);
        }
        public OrderViewModel(string simId, decimal? fixedPrice)
        {
            Init(simId, fixedPrice);
        }
        #endregion

        #region Functions
        public async Task Init(string orderId)
        {
            try
            {
                IsNewOrder = false;
                var response = await ApiHelper.Get<Order>($"api/order/me/{orderId}", true);
                OrderDetail = response.Content as Order;

                var response2 = await ApiHelper.Get<ProfileViewModel>("api/user/profile/" + OrderDetail.BuyerId);
                ProfileViewModel profile = response2.Content as ProfileViewModel;

                Buyer = new ProfileViewModel()
                {
                    Id = OrderDetail.BuyerId,
                    FullName = OrderDetail.FullName,
                    Address = OrderDetail.Address,
                    Phone = OrderDetail.Phone,
                    Email = OrderDetail.Email,
                    Avatar = profile.Avatar,
                };
                Owner = OrderDetail.SimOwner;
                SimDetail = OrderDetail.Sim;
                if (Owner.Id == UserLogged.Id)
                {
                    IsOwner = true;
                }
                else
                {
                    IsOwner = false;
                }
                OrderStatus = OrderDetail.Status;
            }
            catch (Exception ex)
            {
                await Xamarin.Forms.Shell.Current.DisplayAlert(null, "Không tìm thấy 'Đơn hàng' đã chọn", "Đóng");
                await Xamarin.Forms.Shell.Current.Navigation.PopAsync();
            }
        }
        public async Task Init(string simId, decimal? fixedPrice)
        {
            try
            {
                IsNewOrder = true;
                var result = await ApiHelper.Get<Sim>("api/sim/" + simId);
                if (result.IsSuccess)
                {
                    SimDetail = new Sim();
                    SimDetail = (Sim)result.Content;
                    if (fixedPrice != null)
                    {
                        SimDetail.Price = (decimal)fixedPrice;
                    }
                    Owner = SimDetail.Owner;
                    Buyer = new ProfileViewModel
                    {
                        FullName = UserLogged.FullName,
                        Address = UserLogged.Address,
                        Email = UserLogged.Email,
                        Phone = UserLogged.Phone,
                        Avatar = UserLogged.AvatarUrl,
                    };
                    if (Owner.Id == UserLogged.Id)
                    {
                        IsOwner = true;
                    }
                    else
                    {
                        IsOwner = false;
                    }
                    OrderStatus = -1;
                }
            }
            catch (Exception ex)
            {
                await Xamarin.Forms.Shell.Current.DisplayAlert(null, "Không tìm thấy Sim đã chọn", "Đóng");
                await Xamarin.Forms.Shell.Current.Navigation.PopAsync();
            }
        }
        #endregion

        #region Commands
        public Command PlaceOrderCommand
        {
            get => new Command(async () =>
            {
                if (string.IsNullOrWhiteSpace(Buyer.FullName))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng nhập họ tên", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(Buyer.Email))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng nhập Email", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(Buyer.Phone))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng nhập số điện thoại", "OK");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(Buyer.Address))
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng nhập địa chỉ", "OK");
                    return;
                }

                IDictionary data = new Dictionary<string, object>();
                data["SimId"] = SimDetail.Id;
                data["FullName"] = Buyer.FullName;
                data["Email"] = Buyer.Email;
                data["Phone"] = Buyer.Phone;
                data["Address"] = Buyer.Address;
                data["FixedPrice"] = SimDetail.Price;

                ApiResponse response = await ApiHelper.Post("api/order", data, true);
                if (response.IsSuccess)
                {
                    Guid OrderId = Guid.Parse(response.Content.ToString());
                    await Shell.Current.Navigation.PopAsync();
                    await Shell.Current.Navigation.PushAsync(new SimhereApp.Portable.Views.OrderPage(OrderId.ToString()), false);
                    XFToast.ShortMessage("Đặt hàng thành công");
                    await NotificationHelper.SendNotification(
                    new NotificationHelper.NotificationData
                    {
                        SenderId = UserLogged.Id,
                        ToUserId = SimDetail.OwnerId,
                        Message = $"{UserLogged.FullName} Đã đặt sim {SimDetail.DisplayNumber}.",
                        Type = NotificationType.Order,
                        Id = OrderId.ToString()
                    }
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert("", response.Message, "Đóng");
                }
            });
        }
        public Command CancelOrderCommand
        {
            get => new Command(async () =>
            {
                var response = await ApiHelper.Put("api/order/cancel/" + OrderDetail.OrderId, true);
                if (response.IsSuccess)
                {
                    await Shell.Current.Navigation.PopAsync();
                    await Shell.Current.Navigation.PushAsync(new Views.OrderPage(OrderDetail.OrderId.ToString()), false);
                    XFToast.ShortMessage("Huỷ đơn hàng thành công");

                    await NotificationHelper.SendNotification(
                        new NotificationHelper.NotificationData
                        {
                            SenderId = UserLogged.Id,
                            ToUserId = SimDetail.OwnerId,
                            Message = $"{UserLogged.FullName} đã huỷ đơn hàng Sim {SimDetail.DisplayNumber}.",
                            Type = NotificationType.Order,
                            Id = OrderDetail.OrderId.ToString()

                        }
                    );
                }

                else
                {
                    await Shell.Current.DisplayAlert("", response.Message, "Đóng");
                }
            });
        }
        public Command CompleteOrderCommand
        {
            get => new Command(async () =>
            {
                var response = await ApiHelper.Put("api/order/complete/" + OrderDetail.OrderId, true);
                if (response.IsSuccess)
                {
                    await Shell.Current.Navigation.PopAsync();
                    await Shell.Current.Navigation.PushAsync(new Views.OrderPage(OrderDetail.OrderId.ToString()), false);
                    XFToast.ShortMessage($"Đơn hàng Sim {SimDetail.DisplayNumber} đã được hoàn tất");
                    await NotificationHelper.SendNotification(
                       new NotificationHelper.NotificationData
                       {
                           SenderId = UserLogged.Id,
                           ToUserId = Buyer.Id,
                           Message = $"Đơn hàng Sim {SimDetail.DisplayNumber} đã hoàn tất",
                           Type = NotificationType.Order,
                           Id = OrderDetail.OrderId.ToString()
                       }
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert("", response.Message, "Đóng");
                }
            });
        }
        public Command ConfirmOrderCommand
        {
            get => new Command(async () =>
            {
                var response = await ApiHelper.Put("api/order/confirmreceive/" + OrderDetail.OrderId, true);
                if (response.IsSuccess)
                {
                    await Shell.Current.Navigation.PopAsync();
                    await Shell.Current.Navigation.PushAsync(new Views.OrderPage(OrderDetail.OrderId.ToString()), false);
                    XFToast.ShortMessage("Xác nhận thành công");
                    await NotificationHelper.SendNotification(
                       new NotificationHelper.NotificationData
                       {
                           SenderId = UserLogged.Id,
                           ToUserId = Buyer.Id,
                           Message = $"Đơn hàng Sim {SimDetail.DisplayNumber} đã được tiếp nhận",
                           Type = NotificationType.Order,
                           Id = OrderDetail.OrderId.ToString()
                       }
                    );
                }
                else
                {
                    await Shell.Current.DisplayAlert("", response.Message, "Đóng");
                }
            });
        }
        public Command ContactOrderCommand
        {
            get => new Command(async () =>
            {
                var title = "";
                var phoneNumber = "";
                UserLite chatUser;
                if (IsOwner)
                {
                    title = "Liên hệ với người mua";
                    phoneNumber = Buyer.Phone;
                    chatUser = new UserLite()
                    {
                        Id = Buyer.Id,
                        FullName = Buyer.FullName,
                        PictureUrl = Buyer.Avatar
                    };
                }
                else
                {
                    title = "Liên hệ với người bán";
                    chatUser = new UserLite()
                    {
                        Id = Owner.Id,
                        FullName = Owner.FullName,
                        PictureUrl = Owner.PictureUrl
                    };
                    phoneNumber = Owner.PhoneNumber;
                }

                string action = await Shell.Current.DisplayActionSheet(title, "Huỷ", null, "SMS", "Chat", "Gọi");
                if (action == "Chat")
                {
                    await Shell.Current.Navigation.PushAsync(new ChatMessagePage(chatUser));
                }
                else if (action == "SMS")
                {
                    await SendSms($"Tôi đang quan tâm đến số {SimDetail.DisplayNumber} của bạn.", phoneNumber);
                }
                else if (action == "Gọi")
                {
                    PlacePhoneCall(phoneNumber);
                }
            });
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
                Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền truy cập hoặc không hỗ trợ chức năng này.  ", "Đóng");
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
                await Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền truy cập hoặc không hỗ trợ chức năng này.  ", "Đóng");
                // Other error has occurred.
            }
        }
        #endregion
    }
}
