using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SimHere.Entities;

namespace SimhereApp.Portable.ViewModels
{
    public class PostSimViewModel : BaseViewModel
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }
        public ObservableCollection<Carrier> CarrierOptions { get; set; }
        public ObservableCollection<SubcribeType> SubcribeTypeOptions { get; set; }

        public ObservableCollection<SimTypeOption> NumberTypeOptions { get; set; }

        public ObservableCollection<SimDetailImageBackground> Images { get; set; }

        private SimDetailImageBackground _selectedImage;
        public SimDetailImageBackground SelectedImage {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        private AddSimViewModel _sim;
        public AddSimViewModel Sim
        {
            get => _sim;
            set
            {
                if (_sim != value)
                {
                    _sim = value;
                    OnPropertyChanged(nameof(Sim));
                }
            }
        }

        public PostSimViewModel()
        {
            IsLoading = true;
            CarrierOptions = new ObservableCollection<Carrier>(CarrierData.Get());
            SubcribeTypeOptions = new ObservableCollection<SubcribeType>(SubcribeTypeData.Get());
            NumberTypeOptions = new ObservableCollection<SimTypeOption>();
            Images = new ObservableCollection<SimDetailImageBackground>(SimDetailImageBackground.GetImages());
            Sim = new AddSimViewModel();
        }

        public async Task LoadNumberTypes()
        {
            var result = await ApiHelper.Get<List<NumberType>>("api/sim/numbertypes");
            if (result.IsSuccess)
            {
                var numberTypes = result.Content as List<NumberType>;
                var numberTypesCount = numberTypes.Count();
                for (int i = 0; i < numberTypesCount; i++)
                {
                    var numberType = numberTypes[i];

                    NumberTypeOptions.Add(new SimTypeOption(numberType.Name)
                    {
                        Id = numberType.Id
                    });
                }
            }
            else
            {
                throw new Exception("không tim thấy dữ liệu");
            }
        }
    }
}
