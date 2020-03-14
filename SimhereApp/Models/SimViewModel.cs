using SimHere.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimhereApp.Portable.ViewModels
{
    public class SimViewModel : BaseViewModel
    {
        public Sim sim { get; set; }
        bool _isChecked = false;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                if (value)
                    CheckBoxImg = "checkbox_selected.png";
                else
                    CheckBoxImg = "checkbox_circle.png";
                OnPropertyChanged(nameof(IsChecked));
            }
        }
        string _checkBoxImg = "checkbox_circle.png";
        public string CheckBoxImg
        {
            get { return _checkBoxImg; }
            set { _checkBoxImg = value; OnPropertyChanged(nameof(CheckBoxImg)); }
        }
        public SimViewModel(Sim _sim)
        {
            sim = _sim;
        }
    }
}
