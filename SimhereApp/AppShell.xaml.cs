using System;
using System.Threading.Tasks;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public static int appVerion = 1;
        public AppShell()
        {
            InitializeComponent();
            //CheckingAppState().GetAwaiter();
        }
        public async Task CheckingAppState()
        {
            try
            {
                var res = await ApiHelper.Get<string>("api/App");
                if (res.IsSuccess)
                {
                    var ver = int.Parse(res.Content.ToString());
                    if (ver != appVerion)
                    {
                         Application.Current.MainPage = new AppUnavailableStatePage(AppUnavailableStatePage.outDate);
                    }
                }
                else
                {
                    Application.Current.MainPage = new AppUnavailableStatePage(AppUnavailableStatePage.lostConnection);
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage = new AppUnavailableStatePage(33);
            }
        }
    }
}