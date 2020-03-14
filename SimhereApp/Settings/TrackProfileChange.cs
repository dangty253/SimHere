using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace SimhereApp.Portable.Settings
{
    public class TrackProfileChange
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static bool AvatarHasChanged
        {
            get => AppSettings.GetValueOrDefault(nameof(AvatarHasChanged), false);
            set => AppSettings.AddOrUpdateValue(nameof(AvatarHasChanged), value);
        }
    }
}
