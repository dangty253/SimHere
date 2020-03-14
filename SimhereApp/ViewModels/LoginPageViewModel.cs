using System;
using Newtonsoft.Json;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Interfaces;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Services;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        public CustomTabbed_Login customTabbed_Login;

        private IFacebookService _facebookService;

        public Command FacebookLoginCommand { get; protected set; }
        public Command FacebookLogoutCommand { get; protected set; }

        public LoginPageViewModel()
        {
            _facebookService = DependencyService.Get<IFacebookService>();

            FacebookLoginCommand = new Command(FacebookLogin);
            FacebookLogoutCommand = new Command(FacebookLogout);
        }


        private void FacebookLogin()
        {
            DependencyService.Get<IClearCookies>().Clear();
            _facebookService?.Login(OnFacebookLoginCompleted);
        }
        private void FacebookLogout()
        {
            _facebookService?.Logout();
        }

        private async void OnFacebookLoginCompleted(FacebookUser facebookUser, Exception exception)
        {
            if (exception == null)
            {
                SocialLogin<FacebookUser>(facebookUser);
            }
            else
            {
                FacebookLogout();
            }
        }

        public async void SocialLogin<T>(T user) where T : class
        {
            object data = null;
            FacebookUser facebookUser = null;
            GoogleUser googleUser = null;
            ZaloUser zaloUser = null;
            string email = null;

            if (user.GetType() == typeof(FacebookUser))
            {
                facebookUser = user as FacebookUser;
                data = new
                {
                    FullName = facebookUser.FullName,
                    Email = facebookUser.Email,
                    FacebookId = facebookUser.Id,
                    PictureUrl = facebookUser.Picture
                };

                if (!string.IsNullOrEmpty(facebookUser.Email))
                {
                    email = facebookUser.Email;
                }

                if (email == null)
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng cung cấp địa chỉ Email để đăng nhập ", "Đóng");
                    //customTabbed_Login.ShowLoading(false);
                    return;
                }
            }
            else if (user.GetType() == typeof(GoogleUser))
            {
                googleUser = user as GoogleUser;
                data = new
                {
                    FullName = googleUser.Name,
                    Email = googleUser.Email,
                    GoogleId = googleUser.Id,
                    PictureUrl = googleUser.Picture
                };
                if (!string.IsNullOrEmpty(googleUser.Email))
                {
                    email = googleUser.Email;
                }


                if (email == null)
                {
                    await Shell.Current.DisplayAlert("", "Vui lòng cung cấp địa chỉ Email để đăng nhập ", "Đóng");
                    //customTabbed_Login.ShowLoading(false);
                    return;
                }
            }
            if (user.GetType() == typeof(ZaloUser))
            {
                zaloUser = user as ZaloUser;
                data = new
                {
                    FullName = zaloUser.name,
                    ZaloId = zaloUser.id,
                    PictureUrl = zaloUser?.picture?.data?.url ?? "",
                    Birthday = zaloUser.birthday,
                    Sex = zaloUser.gender
                };
            }

            ApiResponse loginResponse = await ApiHelper.Post("api/auth/facebooklogin", data);
            if (loginResponse.IsSuccess)
            {
                AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(loginResponse.Content.ToString());
                UserLogged.UserName = authResponse.Email;
                UserLogged.Password = UserLogged.UserName;
                if (googleUser != null)
                {
                    UserLogged.GoogleId = googleUser.Id;
                }
                else if (facebookUser != null)
                {
                    UserLogged.FacebookId = facebookUser.Id;
                }
                if (zaloUser != null)
                {
                    UserLogged.ZaloId = zaloUser.id;
                }

                UserLogged.SaveLogin(authResponse);
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                string ErrorMessage = loginResponse.GetFirstErrorMessage();
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    ErrorMessage = "Lỗi đăng nhập vui lòng thử lại";
                }
                await Shell.Current.DisplayAlert("", ErrorMessage, "Đóng");
            }
        }
    }
}
