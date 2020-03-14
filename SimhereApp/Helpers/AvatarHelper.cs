using System;
namespace SimhereApp.Portable.Helpers
{
    public class AvatarHelper
    {
        public static string NameToAvatarText(string FullName)
        {
            if (string.IsNullOrWhiteSpace(FullName)) return "";
            if (FullName.Split(' ').Length > 1)
            {
                var splitArr = FullName.Split(' ');
                return (splitArr[0])[0].ToString().ToUpper() + splitArr[splitArr.Length - 1][0].ToString().ToUpper();
            }
            else
            {
                return FullName[0].ToString().ToUpper();
            }
        }

        public static string ToAvatarUrl(string avatar)
        {
            if (string.IsNullOrEmpty(avatar))
            {
                return null;
            }
            if (avatar.Contains("Upload/Avatar"))
            {
                return Configuration.AppConfig.API_IP + avatar;
            }
            return avatar;
        }
        public static string PlaceholderLink(string avatarText)
        {
            return "https://via.placeholder.com/100x100.png/eeeeee/444444?text=" + avatarText;
        }
    }
}
