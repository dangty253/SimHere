using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SimhereApp.Portable.Controls
{
    public partial class LoadingScreen : ContentView
    {
        public static BindableProperty IsLoadingProperty = BindableProperty.Create(nameof(IsLoading), typeof(bool), typeof(LoadingScreen), false, BindingMode.TwoWay);
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
        public LoadingScreen()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.3);
        }
    }
}
