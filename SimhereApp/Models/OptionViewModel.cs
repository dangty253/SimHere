using SimhereApp.Portable.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimhereApp.Portable.Models
{
    public class OptionViewModel : BaseViewModel
    {
        public string Key { get; set; }
        public int Value { get; set; }
        public ICommand Command { get; set; }
        bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                if (value)
                {
                    LabelStyle = (Style)Application.Current.Resources["UnselectedOptionLabel"];
                    FrameStyle = (Style)Application.Current.Resources["UnselectedOptionFrame"];
                }
                else
                {
                    LabelStyle = (Style)Application.Current.Resources["SelectedOptionLabel"];
                    FrameStyle = (Style)Application.Current.Resources["SelectedOptionFrame"];
                }
              
                OnPropertyChanged();
            }
        }
        Style _FrameStyle;
        public Style FrameStyle
        {
            get => _FrameStyle;
            set
            {
                _FrameStyle = value;
                OnPropertyChanged();
            }
        }
        Style _LabelStyle;
        public Style LabelStyle
        {
            get => _LabelStyle;
            set
            {
                _LabelStyle = value;
                OnPropertyChanged();
            }
        }

    }
}
