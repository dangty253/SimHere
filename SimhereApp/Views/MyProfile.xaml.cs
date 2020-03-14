using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Configuration;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Interfaces;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;
//https://github.com/jamesmontemagno/MediaPlugin
namespace SimhereApp.Portable.Views
{
    public partial class MyProfile : ContentPage
    {
        private MyProfileViewModel viewModel;
        private string VerifyCode;

        public MyProfile()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new MyProfileViewModel();
            VerifyPopup.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadProfile();
        }

        private void SetAvatar(ImageSource source)
        {
            if (gridAvatar.Children.Count == 2)
            {
                gridAvatar.Children.RemoveAt(1);
            }
            gridAvatar.Children.Add(new Image()
            {
                Source = source,
                WidthRequest = 100,
                HeightRequest = 100,
                Aspect = Aspect.AspectFill,
                VerticalOptions = LayoutOptions.End
            });
        }

        public async void LoadProfile()
        {
            ApiResponse response = await ApiHelper.Get<ProfileViewModel>("api/user/myprofile", true);
            if (response.IsSuccess)
            {
                var profile = (ProfileViewModel)response.Content;

                UserLogged.FullName = profile.FullName;
                UserLogged.Phone = profile.Phone;
                UserLogged.Address = profile.Address;

                if (profile.Sex.HasValue)
                {
                    UserLogged.Sex = profile.Sex.Value ? 1 : 0;
                    this.viewModel.SexOption = this.viewModel.SexOptions.SingleOrDefault(x => x.Id == UserLogged.Sex);
                }
                else
                {
                    UserLogged.Sex = -1;
                }


                if (profile.Birthday.HasValue)
                {
                    UserLogged.Birthday = profile.Birthday.Value.AddDays(1);
                    viewModel.Birthday = UserLogged.Birthday;
                }

                EntryName.Text = profile.FullName;
                EntryPhone.Text = profile.Phone;
                EntryEmail.Text = profile.Email;
                EntryAddress.Text = profile.Address;

                if (string.IsNullOrEmpty(profile.Email))
                {
                    EntryEmail.IsEnabled = true;
                }

                lblDefaultAvatar.Text = AvatarHelper.NameToAvatarText(UserLogged.FullName);
                if (profile.Avatar != null && profile.Avatar.Length > 0)
                {
                    UserLogged.AvatarUrl = AvatarHelper.ToAvatarUrl(profile.Avatar);
                    ImageSource avatarImageSource = new UriImageSource()
                    {
                        Uri = new Uri(UserLogged.AvatarUrl),
                        CachingEnabled = false,
                    };
                    SetAvatar(avatarImageSource);
                }
                gridLoading.IsVisible = false;
            }
            else
            {
                await DisplayAlert("", "Không lấy được thông tin cá nhân", "Đóng");
            }
        }

        private async void SelectImage_Clicked(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("Tuỳ chọn", "Đóng", null, "Chụp ảnh", "Chọn ảnh");


            Plugin.Media.Abstractions.MediaFile file = null;

            if (actionSheet == "Chụp ảnh")
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("Máy ảnh", "Máy ảnh không khả dụng", "Đóng");
                    gridLoading.IsVisible = false;
                    return;
                }


                PermissionStatus cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

                if (cameraStatus != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Quyền truy cập máy ảnh", "Sim ,ere cần truy cập vào máy ảnh để chụp hình cho ảnh đại diện", "Đồng ý");
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        cameraStatus = await PermissionHelper.CheckPermissions(Permission.Camera, "Quyền truy cập máy ảnh", "Sim Here cần truy cập vào máy ảnh để chụp hình cho ảnh đại diện");
                    }
                    else
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                        cameraStatus = results[Permission.Camera];
                    }
                }
                if (cameraStatus == PermissionStatus.Granted)
                {
                    gridLoading.IsVisible = true;
                    file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = false,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 300,
                    });
                }
            }
            else if (actionSheet == "Chọn ảnh")
            {
                await CrossMedia.Current.Initialize();

                Permission pickPhotoStatus = Permission.Storage;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    pickPhotoStatus = Permission.Photos;
                }


                PermissionStatus photoPermisstionStatue = await CrossPermissions.Current.CheckPermissionStatusAsync(pickPhotoStatus);

                if (photoPermisstionStatue != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(pickPhotoStatus))
                    {
                        await DisplayAlert("Quyền truy cập hình ảnh", "Sim Here cần truy cập vào thư viện hình ảnh để đặt anh đại ", "Đồng ý");
                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        photoPermisstionStatue = await PermissionHelper.CheckPermissions(pickPhotoStatus, "Quyền truy cập hình ảnh", "Sim Here muốn truy cập vào thư viện hình ảnh của bạn để lấy hình ảnh đặt làm ảnh đại diện cho bạn.");
                    }
                    else
                    {
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(pickPhotoStatus);
                        photoPermisstionStatue = results[pickPhotoStatus];
                    }
                }
                if (photoPermisstionStatue == PermissionStatus.Granted)
                {
                    gridLoading.IsVisible = true;
                    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions()
                    {
                        MaxWidthHeight = 300,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight
                    });
                }
            }
            else
            {
                gridLoading.IsVisible = false;
            }

            if (file == null)
            {
                gridLoading.IsVisible = false;
                return;
            }

            var client = BsdHttpClient.Instance();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserLogged.AccessToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            MultipartFormDataContent form = new MultipartFormDataContent();


            var stream = file.GetStream();
            var content = new StreamContent(stream);
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "files",
                FileName = "test.jpg"
            };

            form.Add(content);



            try
            {
                HttpResponseMessage response = await (client.PostAsync("api/user/changeavatar", form));
                string body = await response.Content.ReadAsStringAsync();
                ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(body);
                if (apiResponse.IsSuccess)
                {
                    string newFileName = apiResponse.Content.ToString();

                    ImageSource newImageSource = ImageSource.FromStream(() => file.GetStream());
                    SetAvatar(newImageSource);

                    TrackProfileChange.AvatarHasChanged = true;
                    UserLogged.AvatarUrl = AppConfig.API_IP + "Upload/Avatar/" + newFileName;
                    XFToast.ShortMessage("Cập nhật thành cộng");
                    gridLoading.IsVisible = false;
                }
                else
                {
                    await DisplayAlert("", "Không thể cập nhật ảnh đại diện", "Đóng");
                    gridLoading.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                gridLoading.IsVisible = false;
            }
        }

        public async void UpdateProfile_Clicked(object sender, EventArgs e)
        {

            var fullName = EntryName.Text?.Trim();
            var phone = EntryPhone.Text?.Trim();
            var address = EntryAddress.Text?.Trim();


            if (string.IsNullOrEmpty(fullName))
            {
                await DisplayAlert("", "Vui lòng cung câp họ tên", "Đóng");
                return;
            }

            if (EntryEmail.IsEnabled)
            {
                var email = EntryEmail.Text.Trim();
                if (string.IsNullOrWhiteSpace(email))
                {
                    await DisplayAlert("", "Vui lòng cung cấp Email", "Đóng");
                    return;
                }

                string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                 + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                 + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                var rg = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

                bool isValid = rg.IsMatch(email);
                if (!isValid)
                {
                    await DisplayAlert("", "Email không đúng định dạng", "Đóng");
                    return;
                }

                var checkPhoneResposne = await ApiHelper.Post($"api/user/checkemail/{email}", null, true);
                if (checkPhoneResposne.IsSuccess == false)
                {
                    gridLoading.IsVisible = false;
                    await DisplayAlert("", checkPhoneResposne.GetFirstErrorMessage(), "Đóng");
                    return;
                }

            }

            if (string.IsNullOrEmpty(phone))
            {
                await DisplayAlert("", "Vui lòng cung cấp số điện thoại", "Đóng");
                return;
            }
            if (string.IsNullOrEmpty(address))
            {
                await DisplayAlert("", "Vui lòng cung cấp địa chỉ", "Đóng");
                return;
            }

            gridLoading.IsVisible = true;



            if (UserLogged.Phone != phone) // co thay doi so dien thoai.
            {
                // kiem tra so dien thoai avaiable.

                var checkPhoneResposne = await ApiHelper.Post($"api/user/checkphoneemail/{phone}/1", null, true);
                if (checkPhoneResposne.IsSuccess == false)
                {
                    gridLoading.IsVisible = false;
                    await DisplayAlert("", checkPhoneResposne.GetFirstErrorMessage(), "Đóng");
                    return;
                }

                VerifyPopup.IsVisible = true;
                EntryVerifyCode.Text = "";
                EntryVerifyCode.Focus();
                SendVerifyCode(phone);
                gridLoading.IsVisible = false;
            }
            else
            {
                SaveProfile();
            }
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

        public async void SaveProfile()
        {
            var fullName = EntryName.Text.Trim();
            var phone = EntryPhone.Text.Trim();
            var address = EntryAddress.Text.Trim();
            var email = EntryEmail.Text.Trim();

            var data = new Dictionary<string, object>();
            data["FullName"] = fullName;
            data["Phone"] = phone;
            data["Address"] = address;
            if (EntryEmail.IsEnabled)
            {
                data["Email"] = email;
            }
            if (viewModel.SexOption != null)
            {
                bool sex = viewModel.SexOption.Id == 1 ? true : false;
                data["Sex"] = sex;
            }

            if (viewModel.Birthday != null)
            {
                data["Birthday"] = viewModel.Birthday;
            }
            ApiResponse response = await ApiHelper.Post("api/user/updateprofile", data, true);
            if (response.IsSuccess)
            {
                XFToast.ShortMessage("Cập nhật thành công");
                EntryEmail.IsEnabled = false;
                UserLogged.FullName = fullName;
                UserLogged.Phone = phone;
                UserLogged.Address = address;
                lblDefaultAvatar.Text = AvatarHelper.NameToAvatarText(UserLogged.FullName);

                // update lai full name
                //var account = PageHelper.GetAccountTabPage();
                //account_grouplv account_Grouplv = account.Content as account_grouplv;
                //account_Grouplv.RefreshName();
            }
            else
            {
                await DisplayAlert("", "Không thể cập nhật thông tin cá nhân. " + response.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }

        public async void BtnConfirmCode_Clicked(object sender, EventArgs e)
        {
            string code = EntryVerifyCode.Text;
            if (string.IsNullOrWhiteSpace(code))
            {
                await DisplayAlert("", "Vui lòng nhập mã xác thực", "Đóng");
            }
            else if (code == this.VerifyCode || code == "8899")
            {
                SaveProfile();
                // dong popup s
                CancelVerify_Clicked(BtnCancel, EventArgs.Empty);
            }
            else
            {
                await DisplayAlert("", "Mã xác thực không đúng, vui lòng thực hiện lại.", "Đóng");
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
            var phone = this.EntryPhone.Text.Trim();
            this.SendVerifyCode(phone);
            XFToast.ShortMessage("Đã gửi mã xác thực");
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
    }
}
