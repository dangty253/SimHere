using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SimhereApp.Android.Renderer;
using SimhereApp.Portable.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BsdEntry), typeof(BsdEntryRenderer))]
namespace SimhereApp.Android.Renderer
{
    public class BsdEntryRenderer : Xamarin.Forms.Platform.Android.EntryRenderer
    {
        public BsdEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null)
                return;
            //var gradientDrawable = new GradientDrawable();
            //gradientDrawable.SetCornerRadius(10f);
            //gradientDrawable.SetStroke(1, Android.Graphics.Color.Black);
            //gradientDrawable.SetColor(Android.Graphics.Color.LightGray);
            //Control.SetBackground(gradientDrawable);
            Control.Background = new ColorDrawable(Color.Transparent.ToAndroid());
        }
    }
}