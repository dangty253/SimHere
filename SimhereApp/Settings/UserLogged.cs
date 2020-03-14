using Plugin.FirebasePushNotification;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using System;
using System.Threading.Tasks;

namespace SimhereApp.Portable.Settings
{
    public class UserLogged
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string Id
        {
            get => AppSettings.GetValueOrDefault(nameof(Id), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Id), value);
        }

        public static string UserName
        {
            get => AppSettings.GetValueOrDefault(nameof(UserName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(UserName), value);
        }

        public static string Password
        {
            get => AppSettings.GetValueOrDefault(nameof(Password), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Password), value);
        }

        public static string AccessToken
        {
            get => AppSettings.GetValueOrDefault(nameof(AccessToken), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AccessToken), value);
        }

        public static string FullName
        {
            get => AppSettings.GetValueOrDefault(nameof(FullName), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(FullName), value);
        }

        public static string Email
        {
            get => AppSettings.GetValueOrDefault(nameof(Email), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Email), value);
        }

        public static string Phone
        {
            get => AppSettings.GetValueOrDefault(nameof(Phone), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Phone), value);
        }

        public static DateTime Birthday
        {
            get => AppSettings.GetValueOrDefault(nameof(Birthday), DateTime.MinValue);
            set => AppSettings.AddOrUpdateValue(nameof(Birthday), value);
        }

        public static int Sex
        {
            get => AppSettings.GetValueOrDefault(nameof(Sex), -1);
            set => AppSettings.AddOrUpdateValue(nameof(Sex), value);
        }

        public static string Address
        {
            get => AppSettings.GetValueOrDefault(nameof(Address), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(Address), value);
        }

        public static string AvatarUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(AvatarUrl), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(AvatarUrl), value);
        }

        public static DateTime RegisterDate
        {
            get => AppSettings.GetValueOrDefault(nameof(RegisterDate), DateTime.Now);
            set => AppSettings.AddOrUpdateValue(nameof(RegisterDate), value);
        }

        public static string FacebookId
        {
            get => AppSettings.GetValueOrDefault(nameof(FacebookId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(FacebookId), value);
        }

        public static string GoogleId
        {
            get => AppSettings.GetValueOrDefault(nameof(GoogleId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(GoogleId), value);
        }

        public static string ZaloId
        {
            get => AppSettings.GetValueOrDefault(nameof(ZaloId), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ZaloId), value);
        }

        public static bool IsLogged
        {
            get => Id != string.Empty && AccessToken != string.Empty;
        }

        public static async Task SaveLogin(AuthResponse authResponse)
        {
            Id = authResponse.id;
            AccessToken = authResponse.auth_token;
            FullName = authResponse.FullName;
            Email = authResponse.Email;
            Phone = authResponse.Phone;
            Address = authResponse.Address;
            RegisterDate = authResponse.RegisterDate;
            if (authResponse.Birthday.HasValue)
            {
                Birthday = authResponse.Birthday.Value;
            }
            if (authResponse.Sex.HasValue)
            {
                if (authResponse.Sex.Value)
                {
                    Sex = 1;
                }
                else
                {
                    Sex = 0;
                }
            }
            AvatarUrl = AvatarHelper.ToAvatarUrl(authResponse.AvatarUrl);
            await HubConnHelper.UpdateConnection();
            await App.UpdateUserToken(CrossFirebasePushNotification.Current.Token.ToString());
        }

        public static async Task Logout()
        {
            await App.UpdateUserToken("");
            AppSettings.Clear();
        }
    }
}
