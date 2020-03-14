using System;
using Android.App;
using Android.Widget;
using SimhereApp.Android.Native;
using SimhereApp.Portable.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]
namespace SimhereApp.Android.Native
{
    public class MessageAndroid : IMessage
    {
        public Toast test;
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }

}
