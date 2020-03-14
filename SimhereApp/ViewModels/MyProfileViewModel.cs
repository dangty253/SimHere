using System;
using System.Collections.ObjectModel;
using SimHere.Entities;

namespace SimhereApp.Portable.ViewModels
{
    public class MyProfileViewModel : BaseViewModel
    {
        public ObservableCollection<OptionSet> SexOptions { get; set; }


        private DateTime? _birthday;
        public DateTime? Birthday
        {
            get => _birthday;
            set
            {
                if (_birthday != value)
                {
                    _birthday = value;
                    OnPropertyChanged(nameof(Birthday));
                }
            }
        }

        private OptionSet _sexOption;
        public OptionSet SexOption
        {
            get => _sexOption;
            set
            {
                if (_sexOption != value)
                {
                    _sexOption = value;
                    OnPropertyChanged(nameof(SexOption));
                }
            }
        }

        public int? Sex;
        public MyProfileViewModel()
        {
            SexOptions = new ObservableCollection<OptionSet>();
            SexOptions.Add(new OptionSet(0, "Nữ"));
            SexOptions.Add(new OptionSet(1, "Nam"));
        }
    }
}
