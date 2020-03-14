using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SimhereApp.Portable.Controls
{
    public partial class PopUpLoading : ContentView
    {
        public PopUpLoading()
        {
            InitializeComponent();
            BackgroundColor = Color.FromRgba(0, 0, 0, 0.3);
        }
    }
}
