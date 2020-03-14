using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using SimhereApp.iOS;
using SimhereApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNavigationRenderer))]

namespace SimhereApp.iOS.Renderers
{
    public class CustomNavigationRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            var control = (NavigationPage)this.Element;
            control.BarTextColor = Color.White;
            var gradientLayer = new CAGradientLayer();
            gradientLayer.Bounds = NavigationBar.Bounds;
            gradientLayer.Colors = new CGColor[] { Color.FromHex("#1E5799").ToCGColor(), Color.FromHex("#2989D8").ToCGColor(), Color.FromHex("#1E5799").ToCGColor() };
            gradientLayer.StartPoint = new CGPoint(0.0, 0.5);
            gradientLayer.EndPoint = new CGPoint(1.0, 0.5);

            UIGraphics.BeginImageContext(gradientLayer.Bounds.Size);
            gradientLayer.RenderInContext(UIGraphics.GetCurrentContext());
            UIImage image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
        }
    }
}