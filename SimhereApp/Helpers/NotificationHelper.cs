using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SimhereApp.Portable.Helpers
{
    public static class NotificationHelper
    {
        public static async Task SendNotification(NotificationData data)
        {
            try
            {
                if (App.HubConn != null)
                {
                    await App.HubConn.InvokeAsync("SendNotification", data.SenderId, data.ToUserId,data.Message,data.Type, data.Id);
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        public class NotificationData
        {
            public string SenderId { get; set; }
            public string ToUserId { get; set; }
            public string Message { get; set; }
            public int Type { get; set; }
            public string Id { get; set; }
        }
    }
}
