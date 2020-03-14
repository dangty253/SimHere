using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using SimHere.Entities;
using SimhereApp.Portable.Configuration;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using SimhereApp.Portable.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SimhereApp.Portable.Helpers
{
    public class HubConnHelper
    {
        public async static Task Init()
        {
            if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
                return;
            var userId = string.Empty;
            if (UserLogged.IsLogged)
            {
                userId = UserLogged.Id;
            }

            App.HubConn = new HubConnectionBuilder()
                .WithUrl($"{AppConfig.SIGNALR_POSTHUB_ULR}?userid={userId}").Build();

            #region Actions Listener
            App.HubConn.On<string>("OnAttendedPostChanged", async (message) =>
            {
                await PostHelper.OnAttendedPostChanged(message);
            });
            App.HubConn.On<ChatMessage>("OnReceiveNewChatMessage", async (chatMessage) =>
            {
                await ChatHelper.OnReceiveNewChatMessage(chatMessage);
            });
            #endregion Actions Listener

            await App.HubConn.StartAsync();
        }
        public async static Task Active()
        {
            if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
            {
                await App.HubConn.InvokeAsync("Active");
            }
            else
            {
                await Init();
            }
        }
        public async static Task InActive()
        {
            if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
            {
                await App.HubConn.InvokeAsync("InActive");
            }
        }
        public async static Task UpdateConnection()
        {
            if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
                await App.HubConn.InvokeAsync("UpdateConnection", UserLogged.Id, UserLogged.FullName);
            else
                await Init();
        }
        public async static Task OnClose(Exception error)
        {
            await Task.Delay(1000);
            await App.HubConn.StopAsync();
        }
    }
}
