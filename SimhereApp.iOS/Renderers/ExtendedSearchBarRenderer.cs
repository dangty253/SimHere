using System;
using System.Linq;
using SimhereApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(SearchBar), typeof(ExtendedSearchBarRenderer))]
namespace SimhereApp.iOS.Renderers
{
    public class ExtendedSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            try
            {
                base.OnElementChanged(args);
                UISearchBar bar = (UISearchBar)this.Control;
                bar.TintColor = UIColor.Black;
            }
            catch 
            {

            }
      
        }

        // hide cancel button
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                base.OnElementPropertyChanged(sender, e);

                if (e.PropertyName == "Text")
                {
                    Control.ShowsCancelButton = false;
                }
            }
            catch 
            {

            }
         
        }
    }
}
