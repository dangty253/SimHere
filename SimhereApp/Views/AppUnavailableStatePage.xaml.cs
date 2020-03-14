using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class AppUnavailableStatePage : ContentPage
    {

        public static int lostConnection = 1;
        public static int outDate = 2;
        public AppUnavailableStatePage()
        {
            InitializeComponent();
        }
        public AppUnavailableStatePage(int val)
        {
            InitializeComponent();
            if (val == lostConnection)
            {
                lbl.Text = "Mất kết nối đến máy chủ.";
                btn.Text = "Thử lại";
                btn.Clicked +=  (sender, args) =>
                {
                    Application.Current.MainPage = new AppShell();
                };
            }
            else if(val == outDate)
            {
                lbl.Text = "Hãy cập nhật phiên bản mới để sử dụng SimHere";
                btn.Text = "Thoát";
                btn.Clicked += (sender, args) =>
                {
                    throw new Exception("App is OutDate");
                };
            }
            else 
            {
                lbl.Text = "Mất kết nối đến máy chủ hoặc phiên bản đang sử dụng đã lỗi thời.";
                btn.Text = "Thoát";
                btn.Clicked += (sender, args) =>
                {
                    throw new Exception("App is OutDate");
                };
            }
        }
    }
}
