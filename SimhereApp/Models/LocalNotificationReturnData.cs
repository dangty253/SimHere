using System;
namespace SimhereApp.Portable.Models
{
    /// <summary>
    /// type 1 : tin nhan.
    /// </summary>
    public class LocalNotificationReturnData
    {
        public int Type { get; set; }
        public object Data { get; set; }
        public LocalNotificationReturnData()
        {
        }
    }
}
