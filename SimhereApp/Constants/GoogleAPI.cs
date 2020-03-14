using System;
namespace SimhereApp.Portable.Constants
{
    public class GoogleAPI
    {
        public static string AppName = "SimHereApp";
        // OAuth
        // For Google login, configure at https://console.developers.google.com/
        public static string iOSClientId     = "1007151502786-ak5nli0hrue9nupgiq3ncgklncj2juj1.apps.googleusercontent.com";
        public static string AndroidClientId = "1007151502786-4ccqbio66dgbsmjqifr6fpi4mr4jidfd.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/plus.login";
        public static string DriveApiScope = "https://www.googleapis.com/auth/drive.file https://www.googleapis.com/auth/drive https://www.googleapis.com/auth/drive.readonly";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string iOSRedirectUrl     = "com.googleusercontent.apps.1007151502786-ak5nli0hrue9nupgiq3ncgklncj2juj1:/oauth2redirect";
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.1007151502786-4ccqbio66dgbsmjqifr6fpi4mr4jidfd:/oauth2redirect";
    }
}
