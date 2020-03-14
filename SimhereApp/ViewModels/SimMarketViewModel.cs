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
using SimhereApp.Portable.Models;

namespace SimhereApp.Portable.ViewModels
{
    public class SimMarketViewModel : ListViewPageViewModel<Post>
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
        public class OptionModel : BaseViewModel
        {
            string _key;
            public string Key
            {
                get => _key;
                set
                {
                    _key = value;
                    OnPropertyChanged(nameof(Key));
                }
            }
            int _value;
            public int Value
            {
                get => _value;
                set
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
            bool _isSelected;
            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }
        public ObservableCollection<OptionModel> Filters { get; set; }
        public ICommand SelectedFilterCommand
        {
            get
            {
                return new Command<int>(async (selectedValue) =>
                {
                    foreach (var item in Filters)
                    {
                        item.IsSelected = false;
                        if (item.Value == selectedValue)
                        {
                            item.IsSelected = true;
                        }
                    }
                    await this.ReLoadDataCommand();
                });
            }
        }
        #endregion
        public ICommand CreateNewPostCommand
        {
            get
            {
                return new Command<string>(async (string statusCode) =>
                {
                    //await Shell.Current.Navigation.PushAsync(new CreateNewPostPage(this, int.Parse(statusCode)));
                });
            }
        }
        public SimMarketViewModel()
        {
            Title = "Chợ Sim";
            Filters = new ObservableCollection<OptionModel>
            {
                new OptionModel{ Key = "Tất cả", Value = 0, IsSelected = true},
                new OptionModel{ Key = "Tin Mua", Value = 1, IsSelected = false},
                new OptionModel{ Key = "Tin Bán", Value = 2, IsSelected = false},
                new OptionModel{ Key = "Của tôi", Value = 3, IsSelected = false},
                new OptionModel{ Key = "Theo dõi", Value = 4, IsSelected = false},
            };

            IsLogged = false;
            if (UserLogged.IsLogged)
            {
                IsLogged = true;
                PostViewer = new UserLite { Id = UserLogged.Id, PictureUrl = UserLogged.AvatarUrl, FullName = UserLogged.FullName };
            }

            PreLoadData = new Command(() =>
            {
                int selected = Filters.SingleOrDefault(x => x.IsSelected).Value;
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
            this.LoadData();
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
    }
}
