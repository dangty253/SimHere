using System;
using System.Threading.Tasks;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace SimhereApp.Portable.Helpers
{
    public class PermissionHelper
    {
        public static async Task<PermissionStatus> CheckPermissions(Permission permission,string title,string Description)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
            bool request = false;
            if (permissionStatus == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    var positive = "Cài đặt";
                    var negative = "Để sau";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, Description, positive, negative);
                    if (task == null)
                        return permissionStatus;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }

                    return permissionStatus;
                }

                request = true;

            }

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);

                if (!newStatus.ContainsKey(permission))
                {
                    return permissionStatus;
                }

                permissionStatus = newStatus[permission];

                if (newStatus[permission] != PermissionStatus.Granted)
                {
                    permissionStatus = newStatus[permission];
                    //var question = $"To use the plugin the {permission} permission is required.";
                    var positive = "Cài đăt";
                    var negative = "Để sau";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, Description, positive, negative);
                    if (task == null)
                        return permissionStatus;

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return permissionStatus;
                }
            }

            return permissionStatus;
        }
    }
}
