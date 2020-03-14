using System;
using Facebook.CoreKit;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using Plugin.FirebasePushNotification;
using SimhereApp.iOS.Services;
using SimhereApp.Portable;
using SimhereApp.Portable.Services;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
namespace SimhereApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init();
            DependencyService.Register<IFacebookService, FacebookService>();
            LoadApplication(new App());

            FirebasePushNotificationManager.Initialize(options, true);
            FirebasePushNotificationManager.CurrentNotificationPresentationOption = UNNotificationPresentationOptions.Alert;

            global::Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init(); // google sign
            ImageCircleRenderer.Init();
            return base.FinishedLaunching(app, options);
        }

        public override void OnActivated(UIApplication uiApplication)
        {
            base.OnActivated(uiApplication);
            #region FacebookService
            AppEvents.ActivateApp();
            #endregion
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            // google
            if (url.AbsoluteString.Contains("com.googleusercontent.apps")) // sourceApplication == "com.apple.SafariViewService"
            {
                // Convert NSUrl to Uri
                var uri = new Uri(url.AbsoluteString);

                // Load redirectUrl page
                GoogleSignInAuthenticationState.Authenticator.OnPageLoading(uri);

                return true;
            }
            // facebook
            else
            {
                #region FacebookService
                return ApplicationDelegate.SharedInstance.OpenUrl(app, url, options);
                #endregion
            }
        }

        public override void OnResignActivation(UIApplication application)
        {
            Console.WriteLine("OnResignActivation called, App moving to inactive state.");
        }

        public override void DidEnterBackground(UIApplication uiApplication)
        {
            base.DidEnterBackground(uiApplication);
        }
        public override void WillTerminate(UIApplication uiApplication)
        {
            // tat app
            base.WillTerminate(uiApplication);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            FirebasePushNotificationManager.DidRegisterRemoteNotifications(deviceToken);
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            //base.FailedToRegisterForRemoteNotifications(application, error);
            FirebasePushNotificationManager.RemoteNotificationRegistrationFailed(error);
        }
        // To receive notifications in foregroung on iOS 9 and below.
        // To receive notifications in background in any iOS version
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired 'till the user taps on the notification launching the application.

            // If you disable method swizzling, you'll need to call this method. 
            // This lets FCM track message delivery and analytics, which is performed
            // automatically with method swizzling enabled.
            FirebasePushNotificationManager.DidReceiveMessage(userInfo);
            // Do your magic to handle the notification data
            System.Console.WriteLine(userInfo);

            completionHandler(UIBackgroundFetchResult.NewData);
        }
    }
}
