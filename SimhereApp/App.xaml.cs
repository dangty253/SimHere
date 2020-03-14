using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.FirebasePushNotification;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using Xamarin.Forms;
namespace SimhereApp.Portable
{
    public partial class App : Application
    {
        public static int TabBarHeight { get; set; }
        public static HubConnection HubConn { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (s, p) =>
            {
                await UpdateUserToken(p.Token);
            };
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
            {
                if (p.Data.ContainsKey("type") && p.Data.ContainsKey("id"))
                {
                    object objType;
                    object objId;
                    p.Data.TryGetValue("type", out objType);
                    var type = int.Parse(objType.ToString());
                    p.Data.TryGetValue("id", out objId);
                    var id = objId.ToString();

                    if (type == NotificationType.Post)
                    {
                        var respone = await ApiHelper.Get<Post>($"api/post/{id}", false);
                        if (respone.IsSuccess)
                        {
                            var post = (Post)respone.Content;
                            await Shell.Current.Navigation.PushAsync(new PostPage(post));
                        }
                    }
                    else if (type == NotificationType.Chat)
                    {
                        await Shell.Current.Navigation.PushAsync(new ChatMessagePage(id));
                    }
                    else if (type == NotificationType.Order)
                    {
                        await Shell.Current.Navigation.PushAsync(new OrderPage(id));
                    }
                }
            };
            HubConnHelper.Active();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            HubConnHelper.InActive();
        }
        public async static Task UpdateUserToken(string token)
        {
            await ApiHelper.Post(Configuration.AppConfig.API_IP + $"api/user/updatefbtoken?userid={UserLogged.Id}&token={token}", null);
        }
    }
}