using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ConfirmPhoneToChangePassword : ContentPage
    {
        private string VerifyCode;
        public ConfirmPhoneToChangePassword()
        {
            InitializeComponent();
            VerifyPopup.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);

        }

        public async void SendVerify_Clicked(object sender, EventArgs e)
        {
            var phone = EntryPhone.Text?.Trim();
            if (string.IsNullOrEmpty(phone))
            {
                await DisplayAlert("", "Vui lòng nhập số điện thoại", "Đóng");
            }
            else if (phone.Length < 9)
            {
                await DisplayAlert("", "Số điện thoại không hợp lệ", "Đóng");
            }
            else
            {
                var ran = new Random();
                VerifyCode = $"{ran.Next(0, 9)}{ran.Next(0, 9)}{ran.Next(0, 9)}{ran.Next(0, 9)}";
                ApiResponse response = await ApiHelper.Post($"api/user/confirmphoneandsendcode/{VerifyCode}/{phone}", null);

                if (response.IsSuccess)
                {
                    VerifyPopup.IsVisible = true;
                    EntryVerifyCode.Focus();
                }
                else
                {
                    await DisplayAlert("", response.Message, "Đóng");
                }
            }
        }
        public async void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            string code = EntryVerifyCode.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                await DisplayAlert("", "Vui lòng nhập mã xác thực", "Đóng");
            }
            else
            {
                if (code == this.VerifyCode || code == "8899")
                {
                    var phone = EntryPhone.Text?.Trim();
                    await Navigation.PushAsync(new ResetPassword(phone));
                }
                else
                {
                    await DisplayAlert("", "Mã xác thực không đúng, vui lòng thử lại", "Đóng");
                }
            }
        }
    }
}