using System;
using System.Collections.Generic;
using System.Linq;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class PhanHoiList : ContentPage
    {
        private PhanHoiListViewModel viewModel;
        public PhanHoiList()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new PhanHoiListViewModel();
            Init();
        }
        public async void Init()
        {
            await viewModel.LoadData();
            gridLoading.IsVisible = false;
        }

        private async void BtnSend_Clicked(object sender, EventArgs e)
        {
            gridLoading.IsVisible = true;
            string content = EditorContent.Text?.Trim();
            if (string.IsNullOrEmpty(content))
            {
                await DisplayAlert("", "Vui lòng nhập nội dung phản hồi", "Đóng");
            }
            else
            {
                SimHere.Entities.PhanHoi phanhoi = new SimHere.Entities.PhanHoi();
                phanhoi.MessageContent = content;
                ApiResponse response = await ApiHelper.Post("api/phanhoi", phanhoi, Authenticate: true);
                if (response.IsSuccess)
                {
                    //await DisplayAlert("", "Gửi phản hồi thành công", "Đóng");

                    phanhoi.CreatedDate = DateTime.Now;
                    phanhoi.FromUser = new Users()
                    {
                        FullName = UserLogged.FullName
                    };
                    viewModel.Data.Add(phanhoi);


                    EditorContent.Text = "";
                }
                else
                {
                    await DisplayAlert("", "Lỗi", "Đóng");
                }

            }
            gridLoading.IsVisible = false;
        }
    }
}
