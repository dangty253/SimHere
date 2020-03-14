using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class AddSimByText : ContentPage
    {
        public AddSimByText()
        {
            InitializeComponent();
            gridLoading.IsVisible = false;
        }
        public async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                string text = editor.Text?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(text))
                {
                    await DisplayAlert("", "Vui lòng nhập thông tin sim", "Đóng");
                    return;
                }

                string desciption = @"Số sim bao gồm ký tự số, dấu chấm và khoảng trắng (ví dụ : 0934 055.300)" + Environment.NewLine
                    + @"Giá tiền bao gồm chứ số, dấu chấm, dấu phẩy và ký tự k";

                var actionSheetType = await DisplayActionSheet("Chọn mẫu template bạn đang sử dụng." + desciption, "Huỷ", null, ImportTextTypeDictionary.keys.Select(x => x.Value).ToArray());
                if (actionSheetType == "Huỷ") return;

                // kiem tra xem ket qua chon action sheet co nam trong dictionary khong
                var hasSelectedType = ImportTextTypeDictionary.keys.Where(x => x.Value == actionSheetType).Any();
                if (hasSelectedType == false) return;

                gridLoading.IsVisible = true;

                // lay dictionary item da chon
                var selectedType = ImportTextTypeDictionary.keys.Where(x => x.Value == actionSheetType).SingleOrDefault();

                // gui text dang body, va type dang query string.
                ApiResponse response = await ApiHelper.Post("api/sim/import?type=" + selectedType.Key, text, true);

                if (response.IsSuccess)
                {
                    ImportTextResponse importResponse = JsonConvert.DeserializeObject<ImportTextResponse>(response.Content.ToString());

                    // text hien thi khi co sim bi import lỗi
                    string DescriptionText = importResponse.ErrorCount > 0 ? $"{Environment.NewLine}Các dòng bị lỗi sẽ hiển thị trong ô nhập liệu. Vui lòng sửa và import lại" : "";
                    string actionSheetResponseTitle = $"Có {importResponse.SuccessCount} sim được nhập thành công.{Environment.NewLine}có {importResponse.ErrorCount} sim bị lỗi.{DescriptionText}";


                    // neu co loi
                    if (importResponse.ErrorCount > 0)
                    {
                        // set sim bi loi vao editor cho nguoi dung sua lai 
                        editor.Text = importResponse.ErrorText;
                    }
                    else
                    {
                        editor.Text = "";
                    }


                    // co sim thanh cong thi chuyen de man hinh Active Sim.
                    if (importResponse.SuccessCount > 0)
                    {
                        //gridLoading.IsVisible = false;
                        //var actionSheet = await DisplayActionSheet(actionSheetResponseTitle, "Huỷ", null, "Đăng bán tất cả");

                        //if (actionSheet == "Đăng bán tất cả")
                        //{
                        gridLoading.IsVisible = true;
                        ApiResponse buyAllResponse = await ApiHelper.Put("api/sim/postsimlist", importResponse.SimListAdded.Select(x => x.Id).ToArray());
                        if (response.IsSuccess)
                        {
                            gridLoading.IsVisible = false;
                            //await DisplayAlert("", "Đăng sim thành công", "Đóng");
                            if (importResponse.ErrorCount == 0)
                            {
                                await Navigation.PopAsync();
                            }
                        }
                        else
                        {
                            gridLoading.IsVisible = false;
                            await DisplayAlert("", response.Message, "Đóng");
                        }
                        //}
                        //else
                        //{   
                        //    if (importResponse.ErrorCount == 0)
                        //    {
                        //        await Navigation.PopAsync();
                        //    }
                        //}
                        var currentItem = (Shell.Current.CurrentItem).CurrentItem;
                        currentItem.BindingContext = new MySimListViewModel();
                    }
                    else
                    {
                        gridLoading.IsVisible = false;
                        await DisplayAlert("", actionSheetResponseTitle, "Đóng");
                    }
                }
                else
                {
                    await DisplayAlert("", response.GetFirstErrorMessage() ?? response.Message ?? "", "Đóng");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", ex.Message, "Đóng");
            }
            gridLoading.IsVisible = false;
        }
    }
}
