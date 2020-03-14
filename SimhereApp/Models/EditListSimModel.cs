using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimHere.Entities;

namespace SimhereApp.Portable.Models
{
    public class EditListSimModel : Sim, INotifyPropertyChanged
    {
        private string _displayNumber;
        public new string DisplayNumber
        {
            get => _displayNumber;
            set
            {
                if (_displayNumber != value)
                {
                    if (_displayNumber != null)
                    {
                        IsModified = true;
                    }


                    _displayNumber = value;
                    OnPropertyChanged(nameof(DisplayNumber));
                }
            }
        }


        private Int16 _carrierId;
        public new Int16 CarrierId
        {
            get => _carrierId;
            set
            {
                if (_carrierId != value)
                {
                    if (_carrierId != 0)
                    {
                        IsModified = true;
                    }
                    _carrierId = value;
                    OnPropertyChanged(nameof(CarrierId));
                }

            }
        }

        private decimal? _price;
        public new decimal? Price
        {
            get => _price.HasValue ? _price.Value : 0;
            set
            {
                if (_price != value)
                {
                    if (_price != null)
                    {
                        IsModified = true;
                    }
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        private ICollection<Sim_NumberType> _simNumberType;
        public new ICollection<Sim_NumberType> Sim_NumberTypes
        {
            get => _simNumberType;
            set
            {
                if (_simNumberType != value)
                {
                    if (_simNumberType != null)
                    {
                        IsModified = true;
                    }
                    _simNumberType = value;
                    OnPropertyChanged(nameof(Sim_NumberTypes));
                }

            }
        }

        public bool IsModified { get; set; } = false;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
