using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;

namespace SimhereApp.Portable.ViewModels
{
    public class ImportSimViewModel
    {
        public ObservableCollection<GoogleSheetFileListResponse> GoogleDriveFiles { get; set; }
        public string NextPageToken { get; set; }
        public string Access_Token { get; set; }
        public ImportSimViewModel()
        {
            GoogleDriveFiles = new ObservableCollection<GoogleSheetFileListResponse>();
        }

        public async Task LoadFileFromGoogleDrive()
        {
            if (Access_Token != null)
            {
                string path = $"api/googlesheet/files?access_token={Access_Token}";
                if (!string.IsNullOrEmpty(NextPageToken))
                {
                    path += "&nextPageToken=" + NextPageToken;
                }
                var response = await ApiHelper.Get<GoogleSheetFileRootResponse>(path,true);
                if (response.IsSuccess)
                {
                    GoogleSheetFileRootResponse googleSheetFileRootResponse = response.Content as GoogleSheetFileRootResponse;
                    NextPageToken = googleSheetFileRootResponse.NextPageToken;


                    foreach (var item in googleSheetFileRootResponse.Files)
                    {
                        this.GoogleDriveFiles.Add(item);
                    }
                }
            }
        }
    }
}
