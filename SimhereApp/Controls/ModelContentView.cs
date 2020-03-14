using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SimhereApp.Portable.Controls
{
    public class ModelContentView : ContentView
    {
        public ModelContentView()
        {
            AbsoluteLayout.SetLayoutBounds(this, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(this, AbsoluteLayoutFlags.All);
            this.BackgroundColor = Color.FromRgba(0, 0, 0, 0.4);
            this.Padding = new Thickness(40);
        }
    }
}
