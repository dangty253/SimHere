using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomTabbed_Login : ContentPage
    {
        public CustomTabbed_Login()
        {
            InitializeComponent();
            ShowLoading(false);
        }

        public void click_login(object sender, EventArgs e)
        {
            dangky.IsVisible = false;
            login.IsVisible = true;
            telerik_dangky.BorderThickness = 0;
            telerik_login.BorderThickness = new Thickness(0, 0, 0, 2);
        }

        public void click_dangky(object sender, EventArgs e)
        {
            login.IsVisible = false;
            dangky.IsVisible = true;
            telerik_dangky.BorderThickness = new Thickness(0, 0, 0, 2);
            telerik_login.BorderThickness = 0;
        }

        public void ShowLoading(bool isShow)
        {
            gridLoading.IsVisible = isShow;
        }
    }
}