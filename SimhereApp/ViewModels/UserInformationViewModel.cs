using System.Windows.Input;
using Xamarin.Forms;
using SimhereApp.Portable.Settings;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Views;

namespace SimhereApp.Portable.ViewModels
{
    public class UserInformationViewModel : BaseViewModel
    {
        public bool IsLogged
        {
            get => UserLogged.IsLogged;
        }
        private ProfileViewModel _profile;
        public ProfileViewModel Profile
        {
            get => _profile;
            set
            {
                _profile = value;
                OnPropertyChanged(nameof(Profile));
            }
        }
        public ICommand ButtonLoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var confirm = await Shell.Current.DisplayAlert("Xác nhận", "Bạn muốn đăng xuất?", "Đăng xuất", "Không");
                    if (!confirm) return;
                    UserLogged.Logout();
                    Application.Current.MainPage = new AppShell();
                });
            }
        }
        public ICommand ChatCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                        await Shell.Current.Navigation.PushAsync(new ChatConversationPage());
                });
            }
        }
        public ICommand MyProfileCommand
        {
            get
            {
                return new Command(async () =>
               {
                   if (!IsLogged)
                       await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                   else
                       await Shell.Current.Navigation.PushAsync(new MyProfile());
               });
            }
        }
        public ICommand MyOrdersCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                        await Shell.Current.Navigation.PushAsync(new MyOrders());
                });
            }
        }
        public ICommand FeedbackCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                        await Shell.Current.Navigation.PushAsync(new PhanHoiList());
                });
            }
        }
        public ICommand FollowersCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                        await Shell.Current.Navigation.PushAsync(new IFollowUsers(1));
                });
            }
        }
        public ICommand FollowingUsersCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                        await Shell.Current.Navigation.PushAsync(new IFollowUsers(0));
                });
            }
        }
        public ICommand MySimsCommand
        {
            get
            {
                return new Command(async () =>
                {

                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                    {
                        await Shell.Current.GoToAsync("//homes/mysimlist");
                    }
                });
            }
        }
        public ICommand FollowingSimCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogged)
                        await Shell.Current.Navigation.PushAsync(new CustomTabbed_Login());
                    else
                    {
                        await Shell.Current.Navigation.PushAsync(new Follow());
                    }
                });
            }
        }
        public UserInformationViewModel()
        {
            Title = "Tài khoản";
            if (UserLogged.IsLogged)
            {
                Profile = new ProfileViewModel()
                {
                    Id = UserLogged.Id,
                    FullName = UserLogged.FullName,
                    Avatar = UserLogged.AvatarUrl,
                    RegisterDate = UserLogged.RegisterDate,
                    Address = UserLogged.Address,
                    Birthday = UserLogged.Birthday,
                    Email = UserLogged.Email,
                    Phone = UserLogged.Phone,
                };
            }

        }
    }
}
