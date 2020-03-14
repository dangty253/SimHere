using SimHere.Entities;
using System;

namespace SimhereApp.Portable.ViewModels
{
    public class AddSimViewModel : BaseViewModel
    {
        private string _simNumber;
        public string SimNumber
        {
            get => _simNumber;
            set
            {
                if (_simNumber != value)
                {
                    _simNumber = value;
                    OnPropertyChanged(nameof(SimNumber));
                }
            }
        }

        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        private string _displayNumber;
        public string DisplayNumber
        {
            get => _displayNumber;
            set
            {
                if (_displayNumber != value)
                {
                    _displayNumber = value;
                    OnPropertyChanged(nameof(DisplayNumber));
                }
            }
        }

        private Carrier _carrier;
        public Carrier Carrier
        {
            get => _carrier;
            set
            {
                if (_carrier != value)
                {
                    _carrier = value;
                    OnPropertyChanged(nameof(Carrier));
                }
            }
        }

        private SubcribeType _subcribeType;
        public SubcribeType SubcribeType
        {
            get => _subcribeType;
            set
            {
                if (_subcribeType != value)
                {
                    _subcribeType = value;
                    OnPropertyChanged(nameof(SubcribeType));
                }
            }
        }

        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private DateTime _createdOn;
        public DateTime CreatedOn
        {
            get => _createdOn;
            set
            {
                if (_createdOn != value)
                {
                    _createdOn = value;
                    OnPropertyChanged(nameof(CreatedOn));
                }
            }
        }

        private int _status;
        public int Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private string _statusFormat;
        public string StatusFormat
        {
            get => _statusFormat;
            set
            {
                if (_statusFormat != value)
                {
                    _statusFormat = value;
                    OnPropertyChanged(nameof(StatusFormat));
                }
            }
        }


        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }
    }
}
