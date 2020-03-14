using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SimhereApp.Android.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;
[assembly: ExportRenderer(typeof(SearchBar), typeof(NoUnderlineSearchBarRenderer))]
namespace SimhereApp.Android.Renderer
{
    public class NoUnderlineSearchBarRenderer : SearchBarRenderer
    {
        public NoUnderlineSearchBarRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                LinearLayout linearLayout = this.Control.GetChildAt(0) as LinearLayout;
                linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
                linearLayout = linearLayout.GetChildAt(1) as LinearLayout;

                linearLayout.Background = null; 
            }
        }
        
    }
}