using Newtonsoft.Json;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Services;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentView
    {
        public LoginPageViewModel viewModel;

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new LoginPageViewModel();
            MessagingCenter.Subscribe<ZaloLogin, ZaloUser>(this, "ZaloLoginCallback", async (sender, zaloUser) =>
             {
                 viewModel.SocialLogin<ZaloUser>(zaloUser);
             });
        }


        private async void click_login(object sender, EventArgs e)
        {
            try
            {
                /**/
                if (string.IsNullOrEmpty(entryUsername.Text))
                {
                    await Shell.Current.DisplayAlert("", "Nhập email hoặc số điện thoại", "Đóng");
                    return;

                }
                if (string.IsNullOrEmpty(entryPassword.Text))
                {
                    await Shell.Current.DisplayAlert("", "Nhập mật khẩu", "Đóng");
                    return;
                }

                string userName = entryUsername.Text.Trim();
                string passWord = entryPassword.Text.Trim();

                ApiResponse loginResponse = await ApiHelper.Login(userName, passWord);
                if (loginResponse.IsSuccess)
                {
                    AuthResponse authResponse = JsonConvert.DeserializeObject<AuthResponse>(loginResponse.Content.ToString());
                    UserLogged.UserName = userName;
                    UserLogged.Password = passWord;
                    UserLogged.SaveLogin(authResponse);

                    Application.Current.MainPage = new AppShell();
                }
                else
                {
                    string messageError = loginResponse.GetFirstErrorMessage();
                    if (string.IsNullOrEmpty(messageError))
                    {
                        messageError = "Lỗi đăng nhập, vui lòng thử lại sau.";
                    }
                    await Shell.Current.DisplayAlert("", messageError, "Đóng");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void GoogleLogin_Clicked(object sender, EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = Constants.GoogleAPI.iOSClientId;
                    redirectUri = Constants.GoogleAPI.iOSRedirectUrl;
                    break;

                case Device.Android:
                    clientId = Constants.GoogleAPI.AndroidClientId;
                    redirectUri = Constants.GoogleAPI.AndroidRedirectUrl;
                    break;
            }

            //account = store.FindAccountsForService(Constants.GoogleAPI.AppName).FirstOrDefault();

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.GoogleAPI.Scope,
                new Uri(Constants.GoogleAPI.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.GoogleAPI.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnGoogleAuthCompleted;
            authenticator.Error += OnGoogleAuthError;

            GoogleSignInAuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }


        async void OnGoogleAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnGoogleAuthCompleted;
                authenticator.Error -= OnGoogleAuthError;
            }

            if (e.IsAuthenticated)
            {
                //
                var request = new OAuth2Request("GET", new Uri(Constants.GoogleAPI.UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    string userJson = await response.GetResponseTextAsync();
                    GoogleUser user = JsonConvert.DeserializeObject<GoogleUser>(userJson);
                    viewModel.SocialLogin(user);
                }
                else
                {
                    await Shell.Current.DisplayAlert("", "Không thể kết nối đến Google !", "Đóng");
                }
            }
            else
            {
                //await accountPage.DisplayAlert("", "Không thể kết nối đến Google .", "Đóng");
            }
            await Task.Delay(2000);
        }

        void OnGoogleAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnGoogleAuthCompleted;
                authenticator.Error -= OnGoogleAuthError;
            }
        }
        public async void ZaloLogin_Clicked(object sender, EventArgs e)
        {

                await Shell.Current.Navigation.PushAsync(new ZaloLogin());
   
        }

        public async void ForgotPassword_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ConfirmPhoneToChangePassword());
        }
    }
}