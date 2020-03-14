using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class CreateNewPostViewModel : BaseViewModel
    {
        public SelectSimViewModel selectSimViewModel { get; }
        public PostListViewModel postListViewModel { get; }
        public UserLite User { get; }
        int _type;
        public int Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }
        string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged(nameof(Content));
            }
        }
        public ICommand CreateCommand
        {
            get => new Command(async () =>
            {
                try
                {
                    if (Type == 1)
                    {
                        if (string.IsNullOrEmpty(Content) || string.IsNullOrEmpty(Content.Trim()))
                        {
                            await Shell.Current.DisplayAlert("", "Hãy nhập nội dung bài viết.", "Đóng");
                            return;
                        }
                        var txt = Content.Trim();
                        Content = string.Empty;
                        await postListViewModel.CreateBuyingPost(txt);
                    }
                    else if (Type == 2)
                    {
                        if (await IsReadyToPostAsync())
                        {
                            Sim selectedSim = selectSimViewModel.Data.SingleOrDefault(x => x.IsChecked).sim;
                            await postListViewModel.CreateSellingPost(selectedSim.Id, selectedSim.Price);
                        }
                        else
                        {
                            await Shell.Current.DisplayAlert(null,"Bạn chỉ có thể đăng 1 tin bán trong vòng 60 phút", "Đóng");
                        }
                    }

                    await postListViewModel.ReLoadDataCommand();
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                catch (System.Exception ex)
                {
                    await Shell.Current.DisplayAlert("Không thể tạo bài viết mới", "Hãy thử lại","Đóng");
                }
                
            });
        }
        public bool IsBuying
        {
            get
            {
                if (Type == 1) return true;
                return false;
            }
        }
        public async Task<bool> IsReadyToPostAsync()
        {
            var res = await ApiHelper.Get<List<Post>>($"api/post?UserId={UserLogged.Id}&type={Type}&sortString={"{CreatedOn:-1}"}&take={1}");
            if (res.IsSuccess)
            {
                var list = (List<Post>)res.Content;
                if (list.Count > 0)
                {
                    var item = list.FirstOrDefault();
                    var currentDate = DateTime.UtcNow.AddHours(7);
                    var timeSpan = currentDate.Subtract(item.CreatedOn);
                    if (timeSpan.TotalMinutes < 60.0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public CreateNewPostViewModel() { }
        /// <summary>
        /// type = 1 : Buying \\\\
        /// Type = 2 : Selling
        /// </summary>
        /// <param name="postListVM"></param>
        /// <param name="type"></param>
        public CreateNewPostViewModel(PostListViewModel postListVM, int type)
        {
            Title = "Đăng tin mới";
            User = new UserLite { Id = UserLogged.Id, FullName = UserLogged.FullName, PictureUrl = UserLogged.AvatarUrl };
            postListViewModel = postListVM;
            Type = type;
            if (Type == 2)
            {
                selectSimViewModel = new SelectSimViewModel(1);
                selectSimViewModel.Filter = new FilterModel();
                selectSimViewModel.Filter.OwnerId = UserLogged.Id;
                selectSimViewModel.Filter.Status = 1;
                selectSimViewModel.PreLoadData = new Command(() =>
                {
                    selectSimViewModel.Filter.Keyword = selectSimViewModel.SearchValue;
                    string jsonStringFilterModel = JsonConvert.SerializeObject(selectSimViewModel.Filter);
                    selectSimViewModel.ApiUrl = $"api/sim/filter?Page={selectSimViewModel.Page}&filterModel={jsonStringFilterModel}";
                });
                selectSimViewModel.LoadData().GetAwaiter();
            }
        }
    }
}
