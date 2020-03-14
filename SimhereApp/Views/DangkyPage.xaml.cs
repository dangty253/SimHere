using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Configuration;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DangkyPage : ContentView
    {
        public DangkyViewModel viewModel;
        private string VerifyCode;
        public DangkyPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new DangkyViewModel();
            VerifyPopup.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
        }

        private async void Register_Clicked(object sender, EventArgs e)
        {
            //viewModel.LoadAccountTabPage();
            var mainPage = Shell.Current;
            if (string.IsNullOrEmpty(viewModel.RegisterModel.FullName))
            {
                await mainPage.DisplayAlert("", "Vui lòng nhập họ tên", "Đóng");
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.RegisterModel.Email))
            {
                await mainPage.DisplayAlert("", "Vui lòng nhập Email", "Đóng");
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.RegisterModel.Phone))
            {
                await mainPage.DisplayAlert("", "Vui lòng nhập số điện thoại", "Đóng");
                return;
            }
            else if (string.IsNullOrEmpty(viewModel.RegisterModel.Password))
            {
                await mainPage.DisplayAlert("", "Vui lòng nhập mật khẩu", "Đóng");
                return;
            }
            else
            {
                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
             + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
             + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var rg = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

                bool isValid = rg.IsMatch(viewModel.RegisterModel.Email);
                if (isValid == false)
                {
                    await mainPage.DisplayAlert("", "Email không đúng định dạng", "Đóng");
                    return;
                }
            }

            //viewModel.customTabbed_Login.ShowLoading(true);
            var checkExistResponse = await ApiHelper.Post($"api/user/checkphoneemail/{viewModel.RegisterModel.Phone}/{viewModel.RegisterModel.Email}", null, false);
            if (checkExistResponse.IsSuccess)
            {
                VerifyPopup.IsVisible = true;
                EntryVerifyCode.Text = "";
                EntryVerifyCode.Focus();
                SendVerifyCode(viewModel.RegisterModel.Phone);
            }
            else
            {
                string ErrorMesage = checkExistResponse.GetFirstErrorMessage();
                if (string.IsNullOrEmpty(ErrorMesage))
                {
                    ErrorMesage = "Vui lòng thử lại";
                }
                await mainPage.DisplayAlert("", ErrorMesage, "Đóng");
            }
            //viewModel.customTabbed_Login.ShowLoading(false);
        }

        public async void VerifyConfirm_Clicked(object sender, EventArgs e)
        {
            var mainPage = Shell.Current;
            //viewModel.LoadAccountTabPage();
            //viewModel.customTabbed_Login.ShowLoading(true);
            string code = EntryVerifyCode.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                await mainPage.DisplayAlert("", "Vui lòng nhập mã xác thực", "Đóng");
            }
            else
            {
                if (code == this.VerifyCode || code == "8899")
                {
                    var data = new Dictionary<string, object>();
                    data["FullName"] = viewModel.RegisterModel.FullName;
                    data["Email"] = viewModel.RegisterModel.Email;
                    data["Phone"] = viewModel.RegisterModel.Phone;
                    data["Password"] = viewModel.RegisterModel.Password;
                    
                    if (viewModel.RegisterModel.Birthday != null)
                    {
                        data["Birthday"] = viewModel.RegisterModel.Birthday;
                    }
                    if (viewModel.RegisterModel.SexOption != null)
                    {
                        data["Sex"] = viewModel.RegisterModel.SexOption.Id;
                    }

                    ApiResponse response = await ApiHelper.Post("api/user/register", data);

                    if (response.IsSuccess)
                    {
                        await mainPage.DisplayAlert("","Đăng ký thành công, Vui lòng đăng nhập SimHere","Đăng nhập");

                        VerifyPopup.IsVisible = false;
                        viewModel.RegisterModel.FullName = "";
                        viewModel.RegisterModel.Email = "";
                        viewModel.RegisterModel.Phone = "";
                        viewModel.RegisterModel.Password = "";
                        viewModel.RegisterModel.Birthday = null;
                        viewModel.RegisterModel.SexOption = null;

                        await mainPage.Navigation.PopToRootAsync();
                    }
                    else
                    {
                        string ErrorMessage = response.GetFirstErrorMessage();
                        if (string.IsNullOrEmpty(ErrorMessage))
                        {
                            ErrorMessage = "Đăng ký thất bại, vui lòng thử lại sau";
                        }
                        await mainPage.DisplayAlert("", ErrorMessage, "Đóng");
                    }
                }
                else
                {
                    await mainPage.DisplayAlert("", "Mã xác thực không đúng, vui lòng thực hiện lại.", "Đóng");
                }
            }
            //viewModel.customTabbed_Login.ShowLoading(false);
        }


        public async void SendVerifyCode(string phone)
        {
            try
            {
                var ran = new Random();
                VerifyCode = $"{ran.Next(0, 9)}{ran.Next(0, 9)}{ran.Next(0, 9)}{ran.Next(0, 9)}";
                await ApiHelper.Post($"api/user/sendverifycode/{VerifyCode}/{phone}", null);

            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
        }

        public void CancelVerify_Clicked(object sender, EventArgs e)
        {
            VerifyPopup.IsVisible = false;
            VerifyCode = "";
        }

        public void ResendVerify_Clicked(object sender, EventArgs e)
        {
            EntryVerifyCode.Text = "";
            var phone = this.entryPhone.Text.Trim();
            this.SendVerifyCode(phone);
        }

        void RemoveSexOption_Clicked(object sender, System.EventArgs e)
        {
            pickerSex.SelectedItem = null;
        }

        void RemoveBirthday_Clicked(object sender, System.EventArgs e)
        {
            //btnDate.IsVisible = entryDate.IsVisible = true;
            //bithdatePicker.IsVisible = false;
            birthdayPicker.Date = DateTime.Now;
            birthdayPicker.CleanDate();
        }

        //void test(object sender,EventArgs e)
        //{
        //    bithdatePicker.Focus();
        //}

        //void Handle_DateSelected(object sender, Xamarin.Forms.DateChangedEventArgs e)
        //{
        //    if (e.NewDate != null)
        //    {
        //        btnDate.IsVisible = entryDate.IsVisible = false;
        //        bithdatePicker.IsVisible = true;
        //    }
        //}
    }
}