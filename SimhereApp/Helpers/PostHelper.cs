using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using SimHere.Entities;
using SimhereApp.Portable.Configuration;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using SimhereApp.Portable.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimhereApp.Portable.Helpers
{
    public class PostHelper
    {
        public async static Task WatchingPost(string postId)
        {
            try
            {
                if (App.HubConn != null)
                {
                    await App.HubConn.InvokeAsync("WatchingPost", postId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async static Task StopWatchingPost()
        {
            try
            {
                if (App.HubConn != null)
                {
                    await App.HubConn.InvokeAsync("StopWatchingPost");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
        public async static Task OnAttendedPostChanged(string message)
        {
            try
            {
                var navigationStack = Shell.Current.Navigation.NavigationStack;
                if (navigationStack != null && navigationStack.Any())
                {
                    var last = navigationStack.LastOrDefault();
                    if (last != null && last.GetType() == typeof(PostPage))
                    {
                        var viewModel = last.BindingContext as PostPageViewModel;
                        var newComment = JsonConvert.DeserializeObject<CommentModel>(message);
                        //them vao dau danh sach
                        if (viewModel.MainPost.Id == newComment.ParentId)
                        {
                            viewModel.Data.Insert(0, newComment);
                        }
                        else
                        {
                            //to-do
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public async static Task CreateComment(object content)
        {
            try
            {
                if (App.HubConn != null)
                {
                    string json = JsonConvert.SerializeObject(content);
                    await App.HubConn.InvokeAsync("CreateComment", json);
                }
                else
                    throw new Exception("Mất kết nói với máy chủ. Hãy đăng nhập lại!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
