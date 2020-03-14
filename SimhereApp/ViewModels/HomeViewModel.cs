using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class HomeViewModel : ListViewPageViewModel<Sim>
    {
        public ObservableCollection<Sim> HotSims { get; set; }
        private string _keyword;
        public string Keyword
        {
            get => _keyword;
            set
            {
                if (_keyword != value)
                {
                    _keyword = value;
                    OnPropertyChanged(nameof(Keyword));
                }
            }
        }
        public ICommand SearchCommand
        {
            get
            {
                return new Command(() =>
                {
                    var model = new FilterModel();
                    model.Keyword = this.Keyword;
                    Shell.Current.Navigation.PushAsync(new Views.SearchResult(model));
                });
            }
        }
        public ICommand TapCommand
        {
            get => new Command<Sim>(async (sim) =>
            {
                await Shell.Current.Navigation.PushAsync(new SimDetail(sim.Id));
            });
        }

        public ICommand ReloadHotSims
        {
            get
            {
                return new Command(async () =>
                {
                    HotSims.Clear();
                    await LoadHotSims();
                });
            }
        }
        public async Task LoadHotSims()
        {
            try
            {
                var result = await ApiHelper.Get<List<Sim>>("api/sim/randsims");
                if (result.IsSuccess)
                {
                    var list = (List<Sim>)result.Content;
                    var count = list.Count;
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            var item = list[i];
                            HotSims.Add(item);

                        }
                    }
                }
            }
            catch { }
        }
        public override ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    base.RefreshCommand.Execute(null);
                    ReloadHotSims.Execute(null);
                });
            }
        }
        public HomeViewModel()
        {
            HotSims = new ObservableCollection<Sim>();
            PreLoadData = new Command(() => ApiUrl = $"api/sim?Page={Page}");
            RefreshCommand.Execute(null);
            Title = "Simhere";
        }
    }
}
