using System;
using Android.Webkit;
using SimhereApp.Droid.Native;
using SimhereApp.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(IClearCookiesImplementation))]
namespace SimhereApp.Droid.Native
{
    public class IClearCookiesImplementation : IClearCookies
    {
        public void Clear()
        {
            var cookieManager = CookieManager.Instance;
            cookieManager.RemoveAllCookie();
        }
    }
}