using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using SimhereApp.Portable.Interfaces;
using SimhereApp.Portable.Models;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ZaloLogin : ContentPage
    {
        public long ZALOAPPID = 4601552925479246872;
        public string CallBackUrl = "https://bsdinsight.net/api/auth/getzalocode";
        public string AppScret = "V0K03Lz290BBwECMzlXD";
        public ZaloLogin()
        {
            InitializeComponent();
            DependencyService.Get<IClearCookies>().Clear();
            webView.Source = $"https://oauth.zaloapp.com/v3/auth?app_id={ZALOAPPID}&redirect_uri={CallBackUrl}&state=123";
            webView.Navigated += WebView_Navigating;
        }

        private async void WebView_Navigating(object sender, WebNavigatedEventArgs e)
        {
            gridLoading.IsVisible = false;
            //if (e.Url.StartsWith(Configuration.AppConfig.API_IP, StringComparison.Ordinal))
            {
                try
                {
                    string queryString = e.Url.Replace(CallBackUrl, "");
                    Dictionary<string, string> keyValues = ParseQueryString(queryString);
                    if (keyValues.ContainsKey("code") == false) return;

                    string code = keyValues["code"];
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync($"https://oauth.zaloapp.com/v3/access_token?app_id={ZALOAPPID}&app_secret={AppScret}&code={code}");
                        if (response.IsSuccessStatusCode)
                        {
                            HttpContent auContent = response.Content;
                            string authBody = await auContent.ReadAsStringAsync();
                            ZaloAuthReponse zaloAuthReponse = JsonConvert.DeserializeObject<ZaloAuthReponse>(authBody);


                            var profileResponse = await client.GetAsync($"https://graph.zalo.me/v2.0/me?access_token={zaloAuthReponse.access_token}&fields=id,birthday,name,gender,picture");
                            var profileBody = await profileResponse.Content.ReadAsStringAsync();
                            ZaloUser zaloUser = JsonConvert.DeserializeObject<ZaloUser>(profileBody);

                            MessagingCenter.Send<ZaloLogin, ZaloUser>(this, "ZaloLoginCallback", zaloUser);
                        }
                        else
                        {
                            await DisplayAlert("", "Lỗi hệ thống, vui lòng thử lại.", "Đóng");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("", "Lỗi hệ thống, vui lòng thử lại." + Environment.NewLine + ex.Message, "Đóng");
                }
            }
        }

        public Dictionary<string, string> ParseQueryString(String query)
        {
            Dictionary<String, String> queryDict = new Dictionary<string, string>();
            foreach (String token in query.TrimStart(new char[] { '?' }).Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] parts = token.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2)
                    queryDict[parts[0].Trim()] = HttpUtility.UrlDecode(parts[1]).Trim();
                else
                    queryDict[parts[0].Trim()] = "";
            }
            return queryDict;
        }
    }
}
