using System;
using Android.Content;
using SimhereApp.Droid.Native;
using SimhereApp.Portable.Interfaces;
using Xamarin.Forms;
using AndroidProvider = Android.Provider;
using AndroidApp = Android.App;
using AndroidNet = Android.Net;
using AndroidContent = Android.Content;
[assembly: Xamarin.Forms.Dependency(typeof(SystemSettings))]
namespace SimhereApp.Droid.Native
{
    public class SystemSettings : ISystemSettings
    {
        public void OpenApplicationDetailsSettings()
        {
            Intent settingIntent = new Intent(AndroidProvider.Settings.ActionApplicationDetailsSettings, AndroidNet.Uri.Parse("package:" + AndroidApp.Application.Context.PackageName));
            AndroidApp.Application.Context.StartActivity(settingIntent);
        }
    }
}
