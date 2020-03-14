using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;

namespace SimhereApp.Portable.ViewModels
{
    public class PostPageViewModel : ListViewPageViewModel<CommentModel>
    {
        Post _mainPost;

        public Post MainPost
        {
            get { return _mainPost; }
            set
            {
                _mainPost = value;
                OnPropertyChanged(nameof(MainPost));
            }
        }
        string _avatar;
        public string Avatar
        {
            get { return _avatar; }
            set
            {
                _avatar = value;
                OnPropertyChanged(nameof(Avatar));
            }
        }
        bool isOwner;
        public bool IsOwner
        {
            get => isOwner;
            set
            {
                isOwner = value;
                OnPropertyChanged(nameof(IsOwner));
            }
        }

        public void WatchingThisPost()
        {
            PostHelper.WatchingPost(MainPost.Id).GetAwaiter();
        }
        public void StopWatchingThisPost()
        {
            PostHelper.StopWatchingPost().GetAwaiter();
        }

        public PostPageViewModel(Post post)
        {
            MainPost = post;
            Avatar = MainPost.User.PictureUrl;
            Title = "Bài viết của " + MainPost.User.FullName;
            IsOwner = false;
            if (UserLogged.Id == post.User.Id)
            {
                IsOwner = true;
            }
            PreLoadData = new Command(() =>
            {
                ApiUrl = $"api/comment/getpostcomments?parentId={post.Id}&Page={Page}&sortString={"{CreatedOn:-1}"}";
            });
        }
      
        
        public async Task LoadMoreDataForRealTime()
        {
            var count = Data.Count;
            IsLoadingMore = true;
            if (count != 0 && count % 10 == 0)
            {
                Page++;
            }
            else
            {
                if (Page > 1)
                {
                    Page--;
                }
            }
            OutOfData = false;
            await LoadDataForRealTime();
            IsLoadingMore = false;
        }
        public async Task LoadDataForRealTime()
        {
            PreLoadData.Execute(null);
            var result = await ApiHelper.Get<List<CommentModel>>(ApiUrl, true);
            if (result.IsSuccess)
            {
                var list = (List<CommentModel>)result.Content;
                var count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        if (!Data.Where(x => x.Id == item.Id).Any())
                        {
                            Data.Add(item);
                        }
                    }
                }
                else
                {
                    OutOfData = true;
                }
            }
            else
            {
                Data.Clear();
                Page = 1;
            }
            OnPropertyChanged(nameof(IsEmptyList));
        }
        public async Task AddComment(List<SimViewModel> selectedSims)
        {
            try
            {
                if (selectedSims.Count > 0)
                {
                    CommentModel comment = new CommentModel();
                    comment.ParentId = MainPost.Id;
                    comment.User = new UserLite
                    {
                        Id = UserLogged.Id,
                        FullName = UserLogged.FullName,
                        PictureUrl = UserLogged.AvatarUrl
                    };
                    comment.CreatedOn = DateTime.UtcNow.AddHours(7.0);
                    comment.ModifiedOn = DateTime.UtcNow.AddHours(7.0);
                    comment.AttachedSims = new List<SimLite>();
                    foreach (var selectedSim in selectedSims)
                    {
                        comment.AttachedSims.Add(new SimLite
                        {
                            Id = selectedSim.sim.Id,
                            DisplayNumber = selectedSim.sim.DisplayNumber,
                            Price = selectedSim.sim.Price,
                            SimNumber = selectedSim.sim.SimNumber
                        });
                    }
                    await PostHelper.CreateComment(comment);
                    await NotificationHelper.SendNotification(new NotificationHelper.NotificationData()
                    {
                        Message = $"{comment.User.FullName} đã bình luận bài viết của bạn.",
                        SenderId = comment.User.Id,
                        ToUserId = MainPost.User.Id,
                        Type = NotificationType.Post,
                        Id = MainPost.Id
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
