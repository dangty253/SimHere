using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class ListViewPageViewModel<TEntity> : BaseViewModel where TEntity : class
    {
        public ObservableCollection<TEntity> Data { get; set; }
        public ObservableCollection<SimTypeOption> NumberTypeOptions { get; set; }
        public string ApiUrl { get; set; }
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }
        private FilterModel _filter;
        public FilterModel Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged(nameof(Filter));
            }
        }
        private string _searchValue;
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                var oldValue = _searchValue;
                _searchValue = value;
                OnPropertyChanged(nameof(SearchValue));

                if (!string.IsNullOrWhiteSpace(oldValue) && string.IsNullOrWhiteSpace(value))
                {
                    this.ReLoadDataCommand().GetAwaiter();
                }

            }
        }
        public ICommand SearchBar_Search_Command
        {
            get
            {
                return new Command(() =>
                {
                    this.ReLoadDataCommand().GetAwaiter();
                });
            }
        }
        public bool OutOfData { get; set; } = false;
        private bool _isLoadingMore = false;
        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set
            {
                _isLoadingMore = value;
                OnPropertyChanged(nameof(IsLoadingMore));
            }
        }
        private int _page;
        public int Page
        {
            get => _page;
            set
            {
                if (_page != value)
                {
                    _page = value;
                    OnPropertyChanged(nameof(Page));
                }
            }
        }
        public bool IsEmptyList
        {
            get => Data.Count == 0;
        }
        public ICommand PreLoadData { get; set; }
        
        public virtual ICommand RefreshCommand
        {
            get => new Command(LoadOnRefreshCommand);
        }
        public ListViewPageViewModel()
        {
            Data = new ObservableCollection<TEntity>();
            _page = 1;
        }
        public virtual async Task LoadData()
        {
            PreLoadData.Execute(null);
            var result = await ApiHelper.Get<List<TEntity>>(ApiUrl, true);
            if (result.IsSuccess)
            {
                var list = (List<TEntity>)result.Content;
                var count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        Data.Add(item);
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
        public async Task LoadMoreData()
        {
            if (OutOfData == false)
            {
                IsLoadingMore = true;
                _page += 1;
                OutOfData = false;
                await LoadData();
                IsLoadingMore = false;
            }
        }
        public async void LoadOnRefreshCommand()
        {
            IsRefreshing = true;
            Task.Delay(5000);
            _page = 1;
            Data.Clear();
            OutOfData = false;
            await LoadData();
            IsRefreshing = false;
        }
        public async Task LoadOnRefreshCommandAsync()
        {
            IsRefreshing = true;
            Task.Delay(5000);
            _page = 1;
            Data.Clear();
            OutOfData = false;
            await LoadData();
            IsRefreshing = false;
        }
        public async Task ReLoadDataCommand()
        {
            IsLoadingMore = true;
            _page = 1;
            Data.Clear();
            OutOfData = false;
            await LoadData();
            IsLoadingMore = false;
        }
    }
}
