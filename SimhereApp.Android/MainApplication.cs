

using System;
using Android.App;
using Android.Runtime;
using Android.Util;
using Firebase;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;

namespace FirebasePushSample.Droid
{
    //You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            CrossCurrentActivity.Current.Init(this);

            //Set the default notification channel for your app when running Android Oreo
            if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }

            FirebasePushNotificationManager.NotificationActivityFlags = Android.Content.ActivityFlags.SingleTop;
            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
FirebasePushNotificationManager.Initialize(this,false);
#endif
        }


    }
}

