using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class UserProfileViewModel : ListViewPageViewModel<Sim>
    {
        public string UserId { get; set; }
        public bool IsLogged { get => UserLogged.IsLogged; }
        public bool _isFollowUser;
        public bool IsFollowUser
        {
            get => _isFollowUser;
            set
            {
                _isFollowUser = value;
                OnPropertyChanged(nameof(IsFollowUser));
            }
        }
        private ProfileViewModel _profile;
        public ProfileViewModel Profile
        {
            get => _profile;
            set
            {
                if (_profile != value)
                {
                    _profile = value;
                    OnPropertyChanged(nameof(Profile));
                }
            }
        }
        public async Task GetProfile()
        {
            try
            {
                var response = await ApiHelper.Get<ProfileViewModel>("api/user/profile/" + UserId);
                if (response.IsSuccess)
                {
                    Profile = response.Content as ProfileViewModel;
                }
                else
                {
                    throw new Exception("failed");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(null, "Không tìm thấy thông tin người dùng.", "Đóng");
                await Shell.Current.Navigation.PopAsync();
            }
        }
        public async Task CheckIsFollowThisUser()
        {
            if (UserLogged.IsLogged)
            {
                ApiResponse response = await ApiHelper.Get<object>("api/user/isfollowuser/" + _profile.Id, true);
                if (response.IsSuccess)
                {
                    IsFollowUser = (bool)response.Content;
                }
                return;
            }
            IsFollowUser = false;
        }
        public ICommand FollowCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (UserLogged.IsLogged)
                        {
                            ApiResponse response;
                            if (IsFollowUser)
                                response = await ApiHelper.Post("api/user/unfollowuser/" + Profile.Id, null, true);
                            else
                                response = await ApiHelper.Post("api/user/followuser/" + Profile.Id, null, true);
                            if (response.IsSuccess)
                            {
                                IsFollowUser = !IsFollowUser;
                            }
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng này", "Đóng");
                        }
                    }
                    catch { }
                });
            }
        }
        public ICommand ChatCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (UserLogged.IsLogged)
                    {
                        if (UserLogged.Id != Profile.Id)
                        {
                            await Shell.Current.Navigation.PushAsync(new ChatMessagePage(new UserLite() { Id = Profile.Id, FullName = Profile.FullName, PictureUrl = Profile.Avatar }));
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng này", "Đóng");
                    }
                });
            }
        }
        public UserProfileViewModel(string userId)
        {
            UserId = userId;
            PreLoadData = new Command(() =>
            {
                ApiUrl = $"api/sim/usersim/{userId}?Page={Page}";
            });
            Init();
        }
        public async Task Init()
        {
            await GetProfile();
            await CheckIsFollowThisUser();
            await this.LoadData();
        }
        public UserProfileViewModel()
        {

        }
    }
}
