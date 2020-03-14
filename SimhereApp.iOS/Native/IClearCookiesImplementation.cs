using System;
using System.Runtime.CompilerServices;
using Foundation;
using SimhereApp.iOS.Native;
using SimhereApp.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(IClearCookiesImplementation))]
namespace SimhereApp.iOS.Native
{
    public class IClearCookiesImplementation : IClearCookies
    {
        public void Clear()
        {
            NSHttpCookieStorage CookieStorage = NSHttpCookieStorage.SharedStorage;
            foreach (var cookie in CookieStorage.Cookies)
                CookieStorage.DeleteCookie(cookie);
        }
    }
}
