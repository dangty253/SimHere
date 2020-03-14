using System;
using System.Runtime.CompilerServices;
using Foundation;
using SimhereApp.iOS.Native;
using SimhereApp.Portable.Interfaces;
using UIKit;
[assembly: Xamarin.Forms.Dependency(typeof(SystemSettings))]
namespace SimhereApp.iOS.Native
{
    public class SystemSettings : ISystemSettings
    {
        public void OpenApplicationDetailsSettings()
        {
            var ApplicationUrl = new NSUrl(UIApplication.OpenSettingsUrlString);
            UIApplication.SharedApplication.OpenUrl(ApplicationUrl);
        }
    }
}
