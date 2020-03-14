using SimhereApp.Portable.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SimhereApp.Portable.Models
{
    public class MenuItemModel : BaseViewModel
    {
        public string Name { get; set; }
        public string IconString { get; set; }
        public ICommand Command { get; set; }
        bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                _IsSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
}
