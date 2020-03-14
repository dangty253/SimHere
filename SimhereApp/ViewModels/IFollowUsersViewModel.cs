using System;
using System.Windows.Input;
using SimHere.Entities;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class IFollowUsersViewModel : ListViewPageViewModel<Users>
    {
        private bool _isLoading;
        public bool IsLoading {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public IFollowUsersViewModel(int type)
        {
            IsLoading = true;
            if (type == 0)
            {
                PreLoadData = new Command(() => {
                    ApiUrl = $"api/user/ifollowusers?type=0&page={Page}";
                });

            }
            else
            {
                PreLoadData = new Command(() => {
                    ApiUrl = $"api/user/usersfollowme?type=1&page={Page}";
                });
            }
        }


    }
}
