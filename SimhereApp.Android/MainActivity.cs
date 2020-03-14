
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using SimhereApp.Portable;
using Plugin.CurrentActivity;
using Xamarin.Facebook;
using Xamarin.Forms;
using SimhereApp.Portable.Services;
using Android.Content;
using SimhereApp.Droid.Services;
using Plugin.Permissions;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.FirebasePushNotification;

namespace SimhereApp.Droid
{
    [Activity(Label = "SimhereApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.SetFlags("Shell_Experimental", "Visual_Experimental", "CollectionView_Experimental", "FastRenderers_Experimental");
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            FacebookSdk.SdkInitialize(this);
            global::Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            #region Facebook
            DependencyService.Register<IFacebookService, FacebookService>();
            #endregion
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            ImageCircleRenderer.Init();

            LoadApplication(new App());
            FirebasePushNotificationManager.ProcessIntent(this, Intent);

        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            FirebasePushNotificationManager.ProcessIntent(this, intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            #region FacebookService
            var manager = DependencyService.Get<IFacebookService>();
            if (manager != null)
            {
                (manager as FacebookService).CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            }
            #endregion
        }
    }
}