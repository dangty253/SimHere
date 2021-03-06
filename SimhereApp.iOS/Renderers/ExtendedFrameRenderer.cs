﻿using System;
using CoreGraphics;
using SimhereApp.iOS.Renderers;
using SimhereApp.Portable.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedFrame), typeof(ExtendedFrameRenderer))]
namespace SimhereApp.iOS.Renderers
{
    public class ExtendedFrameRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            //Layer.BorderColor = UIColor.White.CGColor;
            //Layer.CornerRadius = 3;
            //Layer.MasksToBounds = false;
            Layer.ShadowOffset = new CGSize(-2, 2);
            Layer.ShadowRadius = 4;
            Layer.ShadowOpacity = 0.1f;
        }
    }
}
