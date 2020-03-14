using SimhereApp.Portable.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScrollTabOptions : ContentView
    {
        public ScrollTabOptions()
        {
            InitializeComponent();
        }
        public static BindableProperty OptionsSourceProperty = BindableProperty.Create(
                nameof(OptionsSource), typeof(ObservableCollection<OptionViewModel>), typeof(ScrollTabOptions)
            );
        public ObservableCollection<OptionViewModel> OptionsSource
        {
            get => (ObservableCollection<OptionViewModel>)GetValue(OptionsSourceProperty);
            set => SetValue(OptionsSourceProperty, value);
        }
    }
}