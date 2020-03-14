using System;
using System.Collections.Generic;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ResetPassword : ContentPage
    {
        public ResetPassword(string phone)
        {
            InitializeComponent();
        }

        public async void Confirm_Clicked(object sender, EventArgs e)
        {
            string pass = Pass.Text?.Trim();
            string confirmPass = ConfirmPass.Text?.Trim();

            if (string.IsNullOrEmpty(pass) || string.IsNullOrEmpty(confirmPass))
            {
                await DisplayAlert("", "Vui lòng nhập mật khẩu", "Đóng");
                return;
            }
            else if (pass.Length<6 || confirmPass.Length<6)
            {
                await DisplayAlert("", "Mật khẩu phải có ít nhất 6 ký tự", "Đóng");
            }
            else if (pass != confirmPass)
            {
                await DisplayAlert("", "Mật khẩu không khớp", "Đóng");
            }
            else
            {
                ApiResponse response = await ApiHelper.Post("api/user/resetpass", null, false);
                if (response.IsSuccess)
                {
                    await DisplayAlert("", "Cập nhật mật khẩu thành công", "Đóng");
                    await Navigation.PopToRootAsync(false);
                }
                else
                {
                    await DisplayAlert("", response.Message, "Đóng");
                }
            }
        }
    }
}
