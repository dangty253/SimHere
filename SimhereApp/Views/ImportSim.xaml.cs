using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Interfaces;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Services;
using SimhereApp.Portable.ViewModels;
using Xamarin.Auth;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ImportSim : ContentPage
    {
        private Xamarin.Auth.Account account;
        private readonly ImportSimViewModel viewModel;
        public ImportSim()
        {
            InitializeComponent();
            modalDriveFiles.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
            gridLoading.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
            this.BindingContext = viewModel = new ImportSimViewModel();
            ListViewDriveFiles.ItemAppearing += async (sender, e) =>
            {
                var file = e.Item as GoogleSheetFileListResponse;
                if (viewModel.GoogleDriveFiles.Any())
                {
                    if (file.id == viewModel.GoogleDriveFiles.LastOrDefault().id)
                    {
                        if (!string.IsNullOrEmpty(viewModel.NextPageToken))
                        {
                            ListViewFileActivityIndicator.IsRunning = true;
                            await viewModel.LoadFileFromGoogleDrive();
                            ListViewFileActivityIndicator.IsRunning = false;
                        }
                    }
                }
            };
            ListViewDriveFiles.ItemTapped += ImportFile;

            gridLoading.IsVisible = false;
        }

        public async void DownloadGoogleSheetTemplate_Clicked(object sender, EventArgs a)
        {
            gridLoading.IsVisible = true;
            try
            {
                PermissionStatus storagePermission = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);

                if (storagePermission != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Quyền truy cập", "Sim Here cần quyền truy cập vào bộ nhớ để lưu template", "Đồng ý");
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        storagePermission = await PermissionHelper.CheckPermissions(Permission.Storage,"Quyền truy cập bộ nhớ", "Sim Here cần quyền truy cập vào bộ nhớ để lưu template");
                    }
                    else
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                        storagePermission = results[Permission.Storage];
                    }
                }
                if (storagePermission == PermissionStatus.Granted)
                {
                    HttpClient client = BsdHttpClient.Instance();
                    HttpResponseMessage response = await client.GetAsync(Configuration.AppConfig.API_IP + "import_template.xlsx", HttpCompletionOption.ResponseHeadersRead);
                    var streamToReadFrom = await response.Content.ReadAsByteArrayAsync();
                    var service = DependencyService.Get<IFileService>();
                    string folderName = Device.RuntimePlatform == Device.iOS ? "" : "Download/SimHere";
                    service.SaveFile("import_template.xlsx", streamToReadFrom, folderName);
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        service.OpenFile("import_template.xlsx", folderName);
                    }
                    else
                    {
                        XFToast.ShortMessage("Tải template thành công");
                    }
                }
            }
            catch
            {
                await DisplayAlert("", "Tải thất bại", "Đóng");
            }
            gridLoading.IsVisible = false;
        }

        public void SignInGoogleDrive_Clicked(object sender, System.EventArgs e)
        {
            gridLoading.IsVisible = true;
            if (viewModel.Access_Token == null)
            {
                GoolgeAuthentication();
            }
            else
            {
                modalDriveFiles.IsVisible = true;
                gridLoading.IsVisible = false;
            }
        }

        public void GoolgeAuthentication()
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

            var authenticator = new OAuth2Authenticator(
                clientId,
                null,
                Constants.GoogleAPI.DriveApiScope,
                new Uri(Constants.GoogleAPI.AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(Constants.GoogleAPI.AccessTokenUrl),
                null,
                true);

            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;

            GoogleSignInAuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);
        }

        public async void ImportFile(object sender, ItemTappedEventArgs e)
        {
            gridLoading.IsVisible = true;
            var file = e.Item as GoogleSheetFileListResponse;
            var response = await ApiHelper.Post($"api/googlesheet/import/{file.id}?access_token={viewModel.Access_Token}", null, true);
            if (response.IsSuccess)
            {
                modalDriveFiles.IsVisible = false;
                XFToast.ShortMessage("Import thành công");
            }
            else
            {
                await DisplayAlert("", response.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }
        private async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

            if (e.IsAuthenticated)
            {
                account = e.Account;
                string access_token = account.Properties["access_token"];

                viewModel.Access_Token = access_token;
                viewModel.GoogleDriveFiles.Clear();
                await viewModel.LoadFileFromGoogleDrive();
                modalDriveFiles.IsVisible = true;
            }
            else
            {
                // huy ko dang nhap
            }

            gridLoading.IsVisible = false;
        }
        private async void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
                await DisplayAlert("", "Không thể kết nối đến Google Drive.", "Đóng");
            }
        }

        private void HideModalDriveFiles_Clicked(object sender, System.EventArgs e)
        {
            modalDriveFiles.IsVisible = false;
        }
    }
}

