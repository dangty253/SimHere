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
    public class ChatHelper
    {
        public async static Task CreateComment(object content)
        {
            try
            {
                if (App.HubConn != null)
                {
                    string json = JsonConvert.SerializeObject(content);
                    await App.HubConn.InvokeAsync("CreateComment", json);
                }
                else
                    throw new Exception("Mất kết nói với máy chủ. Hãy đăng nhập lại!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task CreateNewChatMessage(ChatMessage chatMessage)
        {
            try
            {
                if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
                {
                    await App.HubConn.InvokeAsync("CreateNewChatMessage", chatMessage);
                }
                else
                    throw new Exception("Mất kết nói với máy chủ. Hãy đăng nhập lại!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task CreateChatConversation(ChatPackage package)
        {
            try
            {
                if (App.HubConn != null && App.HubConn.State == HubConnectionState.Connected)
                {
                    await App.HubConn.InvokeAsync("CreateChatConversation", package);
                }
                else
                    throw new Exception("Mất kết nói với máy chủ. Hãy đăng nhập lại!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task OnReceiveNewChatMessage(ChatMessage chatMessage)
        {
            try
            {
                var navigationStack = Shell.Current.Navigation.NavigationStack;
                if (navigationStack != null && navigationStack.Any())
                {
                    var respone = await ApiHelper.Get<ChatConversation>($"api/chat/conversation?conversationid={chatMessage.ConversationId}");
                    if (respone.IsSuccess)
                    {
                        var chatConversation = respone.Content as ChatConversation;
                        var chatConversationModel = new ChatConversationModel
                        {
                            Id = chatConversation.Id,
                            LatestContent = chatConversation.LatestContent,
                            CreatedOn = chatConversation.CreatedOn,
                            ModifiedOn = chatConversation.ModifiedOn,
                        };

                        if (chatConversation.User1.Id == UserLogged.Id)
                        {
                            chatConversationModel.Receiver = chatConversation.User2;
                        }
                        else
                        {
                            chatConversationModel.Receiver = chatConversation.User1;
                        }
                        #region Update ChatConversation
                        //Check if conversationPage is opened?
                        var conversationPage = navigationStack.SingleOrDefault(x => x != null && x.GetType() == typeof(ChatConversationPage));
                        if (conversationPage != null)
                        {
                            var viewModel = conversationPage.BindingContext as ChatConversationViewModel;
                            var conv = viewModel.Data.FirstOrDefault(x => x.Id == chatConversationModel.Id);
                            //Check If Conversation of chatMessage is loaded
                            //if loaded => update view data
                            if (conv != null)
                            {
                                conv.LatestContent = chatConversationModel.LatestContent;
                                conv.ModifiedOn = chatConversationModel.ModifiedOn;
                            }
                            //else add conversation to list
                            else
                            {
                                viewModel.Data.Insert(0, chatConversationModel);
                            }
                        }
                        #endregion

                        #region Update ChatMessage
                        //add message to chatmessagepage if opened
                        var chatMessagePages = navigationStack.Where(x => x != null && x.GetType() == typeof(ChatMessagePage));
                        if (chatMessagePages != null && chatMessagePages.Any())
                        {
                            foreach (ChatMessagePage chatMessagePage in chatMessagePages)
                            {
                                var viewModel = chatMessagePage.BindingContext as ChatMessageViewModel;
                                //them vao dau danh sach
                                if ((viewModel.ConversationId == chatConversationModel.Id)
                                    || viewModel.Receiver.Id == chatConversationModel.Receiver.Id)
                                {
                                    if (viewModel.IsNewConversation)
                                    {
                                        viewModel.IsNewConversation = false;
                                        viewModel.ConversationId = chatConversationModel.Id;
                                        await viewModel.Initilize();
                                    }
                                    else
                                    {
                                        viewModel.Data.Insert(0, chatMessage);
                                    }
                                }
                                else
                                {
                                    //to-do
                                }
                            }
                        }
                        #endregion
                    }
                }
            }
            catch
            {

            }
        }
    }
}