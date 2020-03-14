using SimHere.Entities;
using SimHere.Entities.Response;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using SimhereApp.Portable.Views;
using System.Collections.Generic;
using Xamarin.Essentials;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Models;

namespace SimhereApp.Portable.ViewModels
{
    public class PostListViewModel : ListViewPageViewModel<Post>
    {
        bool islogged;
        public bool IsLogged { get => islogged; set { islogged = value; OnPropertyChanged(nameof(IsLogged)); } }
        bool isInputEmpty;
        public bool IsInputEmpty { get => isInputEmpty; set { isInputEmpty = value; OnPropertyChanged(nameof(IsInputEmpty)); } }
        UserLite postViewer;
        public UserLite PostViewer
        {
            get => postViewer;
            set
            {
                postViewer = value;
                OnPropertyChanged(nameof(postViewer));
            }
        }
        #region Filter
        public ObservableCollection<MenuItemModel> MenuItems { get; set; }
        public ObservableCollection<OptionViewModel> Options { get; set; }

        #endregion
        public ICommand OpenUserProfile
        {
            get => new Command<Post>(async (post) =>
            {
                await Shell.Current.Navigation.PushAsync(new UserProfile(post.User.Id));
            });
        }
        public ICommand CreateBuyingPostCommand
        {
            get => new Command(async () =>
               {
                   if (UserLogged.IsLogged)
                   {
                       await Shell.Current.Navigation.PushAsync(new CreateNewPostPage(this, 1));
                   }
                   else
                   {
                       await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                       await Shell.Current.GoToAsync("//homes/account");
                   }
               });
        }
        public ICommand CreateSellingPostCommand
        {
            get => new Command(async () =>
               {
                   if (UserLogged.IsLogged)
                   {
                       await Shell.Current.Navigation.PushAsync(new CreateNewPostPage(this, 2));
                   }
                   else
                   {
                       await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                       await Shell.Current.GoToAsync("//homes/account");
                   }
               });
        }
        public ICommand OpenPostOptionsCommand
        {
            get
            {
                return new Command<Post>(async (Post post) =>
               {
                   if (post.Type == 1)
                   {
                       var rs = await Shell.Current.DisplayActionSheet(null, "Đóng", null, new string[] { "Chi tiết" });
                       switch (rs)
                       {
                           case "Chi tiết":
                               {

                                   break;
                               }
                           default: break;
                       }
                   }
                   else if (post.Type == 2)
                   {
                       var rs = await Shell.Current.DisplayActionSheet(null, "Đóng", null, new string[] { "Chi tiết", "Liên hệ người bán", "Đặt hàng" });
                       switch (rs)
                       {
                           case "Chi tiết":
                               {
                                   await Shell.Current.Navigation.PushAsync(new SimDetail(post.SellingSim.Id, post.SellingSim.Price));
                                   break;
                               }
                           case "Liên hệ người bán":
                               {
                                   await Shell.Current.Navigation.PushAsync(new UserProfile(post.User.Id));
                                   break;
                               }
                           case "Đặt hàng":
                               {
                                   await Shell.Current.Navigation.PushAsync(new Views.OrderPage(post.SellingSim.Id, post.SellingSim.Price));
                                   break;
                               }
                           default: break;
                       }
                   }
               });
            }
        }
        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<Post>(async (Post post) =>
                {
                    if (post != null)
                    {
                        if (post.Type == 1)
                            await Shell.Current.Navigation.PushAsync(new PostPage(post));
                        else if (post.Type == 2)
                            await Shell.Current.Navigation.PushAsync(new SimDetail(post.SellingSim.Id, post.SellingSim.Price));
                    }
                });
            }
        }
        public ICommand CallCommand
        {
            get
            {
                return new Command<Post>(async (Post post) =>
                {
                    if (IsLogged)
                    {
                        if (post.Type == 2 && PostViewer.Id != post.User.Id)
                        {
                            var res = await ApiHelper.Get<ProfileViewModel>($"api/user/profile/{post.User.Id}");
                            if (res.IsSuccess)
                            {
                                var user = res.Content as ProfileViewModel;
                                if (!string.IsNullOrWhiteSpace(user.Phone))
                                {
                                    PlacePhoneCall(user.Phone);
                                }
                                else
                                {
                                    await Shell.Current.DisplayAlert(null, "Người dùng chưa thiết lập số điện thoại liên lạc", "Đóng");
                                }
                            }
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                });
            }
        }
        public ICommand SendMessageCommand
        {
            get
            {
                return new Command<Post>(async (Post post) =>
                {
                    if (IsLogged)
                    {
                        if (post.Type == 2 && PostViewer.Id != post.User.Id)
                        {
                            string action = await Shell.Current.DisplayActionSheet("Bạn muốn nhắn tin qua số điện thoại hay chat ?", "Huỷ", null, "SMS", "Chat");
                            if (action == "Chat")
                            {
                                //var result = await ApiHelper.Get<Sim>("api/sim/" + post.SellingSim.Id);
                                //if (result.IsSuccess)
                                //{
                                //    var sim = (Sim)result.Content;
                                //    await Shell.Current.Navigation.PushAsync(new ChatDetail(sim.Owner));

                                //}
                                await Shell.Current.Navigation.PushAsync(new ChatMessagePage(post.User));
                            }
                            else if (action == "SMS")
                            {
                                var res = await ApiHelper.Get<ProfileViewModel>($"api/user/profile/{post.User.Id}");
                                if (res.IsSuccess)
                                {
                                    var user = res.Content as ProfileViewModel;
                                    if (!string.IsNullOrWhiteSpace(user.Phone))
                                    {
                                        await SendSms($"Xin chào {user.FullName}.{Environment.NewLine}Tôi đang quan tâm đến sim số {post.SellingSim.DisplayNumber} của bạn.", user.Phone);
                                    }
                                    else
                                    {
                                        await Shell.Current.DisplayAlert(null, "Người dùng chưa thiết lập số điện thoại liên lạc", "Đóng");
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                });
            }
        }
        public ICommand OrderCommand
        {
            get
            {
                return new Command<Post>(async (Post post) =>
                {
                    if (IsLogged)
                    {
                        if (post.Type == 2 && PostViewer.Id != post.User.Id)
                        {
                            await Shell.Current.Navigation.PushAsync(new Views.OrderPage(post.SellingSim.Id, post.SellingSim.Price));
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(null, "Bạn cần đăng nhập để thực hiện chức năng  này", "Đóng");
                        await Shell.Current.GoToAsync("//homes/account");
                    }
                });
            }
        }
        public Post BasePost()
        {
            Post post = new Post();
            post.User = new UserLite
            {
                Id = UserLogged.Id,
                FullName = UserLogged.FullName,
                PictureUrl = UserLogged.AvatarUrl
            };
            post.CreatedOn = DateTime.UtcNow.AddHours(7.0);
            post.ModifiedOn = DateTime.UtcNow.AddHours(7.0);
            return post;
        }
        public async Task CreateSellingPost(string simId, decimal price)
        {
            var post = BasePost();
            post.CreatedOn = DateTime.UtcNow.AddHours(7.0);
            post.ModifiedOn = DateTime.UtcNow.AddHours(7.0);
            post.Type = 2;

            var result = await ApiHelper.Get<Sim>("api/sim/" + simId);
            Sim rs = result.Content as Sim;

            post.SellingSim = new SimLite
            {
                Id = rs.Id
                ,
                DisplayNumber = rs.DisplayNumber
                ,
                SimNumber = rs.SimNumber
                ,
                CarrierId = rs.CarrierId
                ,
                Price = price
            };

            if (rs.Sim_NumberTypes.Count > 0)
            {
                var types = new List<short>();
                foreach (var item in rs.Sim_NumberTypes)
                {
                    types.Add(item.NumberTypeId);
                }
                post.SellingSim.Sim_NumberTypes = types;
            }
            await AddNewPost(post);
        }
        public async Task CreateBuyingPost(string postContent)
        {
            var post = BasePost();
            post.Type = 1;
            post.Content = postContent;
            await AddNewPost(post);
        }
        public async Task AddNewPost(Post post)
        {
            try
            {
                ApiResponse response = await ApiHelper.Post("api/post", post);
                if (response.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("", "Tạo bài viết thành công.", "Đóng");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public ICommand OnSelectedCommand
        {
            get => new Command<OptionViewModel>(async (item) =>
            {
                var selectedItem = Options.Where(x => x.IsSelected == true);
                foreach (var x in selectedItem)
                {
                    x.IsSelected = false;
                }
                item.IsSelected = true;
                await this.ReLoadDataCommand();
            });
        }
        public void PlacePhoneCall(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (Exception ex)
            {
                // Other error has occurred.
                Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền hoặc không hỗ trợ chức năng này.  ", "Đóng");
            }
        }
        public async Task SendSms(string messageText, string recipient)
        {
            try
            {
                var message = new SmsMessage(messageText, new[] { recipient });
                await Sms.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(null, "Thiết bị của bạn không có quyền hoặc không hỗ trợ chức năng này.  ", "Đóng");
                // Other error has occurred.
            }
        }
        public PostListViewModel()
        {
            Title = "Chợ Sim";
            Options = new ObservableCollection<OptionViewModel>
            {
                new OptionViewModel{ Key = "Tất cả", Value = 0, IsSelected = true, Command = OnSelectedCommand},
                new OptionViewModel{ Key = "Tin Mua", Value = 1, IsSelected = false, Command = OnSelectedCommand},
                new OptionViewModel{ Key = "Tin Bán", Value = 2, IsSelected = false, Command = OnSelectedCommand},
                new OptionViewModel{ Key = "Của tôi", Value = 3, IsSelected = false, Command = OnSelectedCommand},
            };
            MenuItems = new ObservableCollection<MenuItemModel>
            {
                new MenuItemModel{ Name = "Đăng mua", IconString= "\uf7c4", Command = CreateBuyingPostCommand},
                new MenuItemModel{ Name = "Đăng bán", IconString= "\uf7c4", Command = CreateSellingPostCommand},
                new MenuItemModel{ Name = "Tải lại trang", IconString= "\uf2f9", Command = RefreshCommand},
            };

            IsLogged = false;
            if (UserLogged.IsLogged)
            {
                IsLogged = true;
                PostViewer = new UserLite { Id = UserLogged.Id, PictureUrl = UserLogged.AvatarUrl, FullName = UserLogged.FullName };
            }

            PreLoadData = new Command(() =>
            {
                int selected = Options.SingleOrDefault(x => x.IsSelected).Value;
                if (selected == 3)
                {
                    ApiUrl = $"api/post?Page={Page}&UserId={UserLogged.Id}";
                }
                else if (selected == 1 || selected == 2)
                {
                    ApiUrl = $"api/post?Page={Page}&type={selected}";
                }
                else
                {
                    ApiUrl = $"api/post?Page={Page}";
                }

            });
            this.RefreshCommand.Execute(null);
        }
    }
}
