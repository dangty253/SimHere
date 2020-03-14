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
using Android;
using ListView = Android.Widget.ListView;

[assembly: ExportRenderer(typeof(Xamarin.Forms.ListView), typeof(ListViewScrollRenderer))]
namespace SimhereApp.Android.Renderer
{
    public class ListViewScrollRenderer : ListViewRenderer
    {
        public ListViewScrollRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            //if (e.NewElement != null)
            //{
            //    var listView = this.Control as ListView;
            //    listView.NestedScrollingEnabled = true;
            //}
        }
    }
}