using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace SimhereApp.Portable.ViewModels
{
    public class DangkyViewModel : BaseViewModel
    {
        //Chon gio tinh form KHACHHANG

        public ObservableCollection<OptionSet> SexOptions { get; set; }

        public RegisterViewModel RegisterModel { get; set; }

        public CustomTabbed_Login customTabbed_Login;

        public DangkyViewModel()
        {
            SexOptions = new ObservableCollection<OptionSet>()
            {
                new OptionSet(1,"Nam"),
                new OptionSet(0,"Nữ")
                          };
            RegisterModel = new RegisterViewModel();
        }
    }
}
