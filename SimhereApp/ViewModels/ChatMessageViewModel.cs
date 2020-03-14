using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using SimHere.Entities;
using SimHere.Entities.ViewModels;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{

    public class ChatMessageViewModel : ListViewPageViewModel<ChatMessage>
    {
        public bool IsNewConversation { get; set; }
        public UserLite Sender { get; }
        UserLite _Receiver;
        public UserLite Receiver
        {
            get
            {
                return _Receiver;
            }
            set
            {
                _Receiver = value;
                OnPropertyChanged(nameof(Receiver));
            }
        }
        public string ConversationId { get; set; }
        string _newMessage;
        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged(nameof(NewMessage));
            }
        }
        public ICommand OnSendNewMessage
        {
            get
            {
                return new Command(async () =>
                {
                    if (!string.IsNullOrWhiteSpace(NewMessage))
                    {
                        if (!IsNewConversation)
                        {
                            var chatMessage = new ChatMessage
                            {
                                ConversationId = ConversationId,
                                Content = NewMessage,
                                SenderId = Sender.Id,
                            };
                            await ChatHelper.CreateNewChatMessage(chatMessage);
                        }
                        else
                        {
                            ChatPackage package = new ChatPackage { Content = NewMessage, Sender = Sender, Receiver = Receiver };
                            await ChatHelper.CreateChatConversation(package);
                        }
                        NewMessage = string.Empty;
                    }
                });
            }
        }
        public ChatMessageViewModel()
        {

        }
        public ChatMessageViewModel(UserLite receiver)
        {
            Title = "Chat";
            IsNewConversation = true;

            Sender = new UserLite { Id = UserLogged.Id, FullName = UserLogged.FullName, PictureUrl = UserLogged.AvatarUrl };
            receiver.PictureUrl = AvatarHelper.ToAvatarUrl(receiver.PictureUrl);
            Receiver = receiver;
            GetConversation();
        }
        public ChatMessageViewModel(string conversionId)
        {
            Title = "Chat";
            IsNewConversation = false;

            ConversationId = conversionId;
            Sender = new UserLite { Id = UserLogged.Id, FullName = UserLogged.FullName, PictureUrl = UserLogged.AvatarUrl };
            GetConversation(conversionId);
        }
        public async Task Initilize()
        {
            PreLoadData = new Command(() =>
            {
                ApiUrl = $"api/chat/message?conversationId={ConversationId}&Page={Page}";
            });
            await LoadData();
        }
        public async Task GetConversation(string conversationId)
        {
            var respone = await ApiHelper.Get<ChatConversation>($"api/chat/conversation?conversationid={conversationId}");
            if (respone.IsSuccess)
            {
                var conv = respone.Content as ChatConversation;
                if (!string.IsNullOrWhiteSpace(conv.Id))
                {
                    ConversationId = conv.Id;
                    IsNewConversation = false;
                    var receiverId = "";
                    if (conv.User1.Id == UserLogged.Id)
                    {
                        receiverId = conv.User2.Id;
                    }
                    else
                    {
                        receiverId = conv.User1.Id;
                    }
                    var res = await ApiHelper.Get<ProfileViewModel>($"api/user/profile/{receiverId}");
                    if (res.IsSuccess)
                    {
                        var user = res.Content as ProfileViewModel;
                        Receiver = new UserLite() { Id = user.Id, FullName = user.FullName, PictureUrl = AvatarHelper.ToAvatarUrl(user.Avatar) };
                    }

                    await Initilize();
                }
            }
        }
        public async Task GetConversation()
        {
            var respone = await ApiHelper.Get<ChatConversation>($"api/chat/conversation/getone?senderid={Sender.Id}&receiverid={Receiver.Id}");
            if (respone.IsSuccess)
            {
                var conv = respone.Content as ChatConversation;
                if (!string.IsNullOrWhiteSpace(conv.Id))
                {
                    ConversationId = conv.Id;
                    IsNewConversation = false;
                    await Initilize();
                }
            }
        }
        public override async Task LoadData()
        {
            PreLoadData.Execute(null);
            var result = await ApiHelper.Get<List<ChatMessage>>(ApiUrl, true);
            if (result.IsSuccess)
            {
                var list = (List<ChatMessage>)result.Content;
                var count = list.Count;
                var countData = Data.Count;
                ChatMessage lastItem;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        if (countData > 0)
                        {
                            lastItem = Data[countData - 1];
                            if (DateTime.Compare(item.CreatedOn.Date, lastItem.CreatedOn.Date) != 0)
                            {
                                Data.Add(new ChatMessage() { CreatedOn = lastItem.CreatedOn });
                                countData++;
                            }

                        }
                        if ((i + 1) < count)
                        {
                            var next = list[i + 1];
                            if (next.SenderId != item.SenderId)
                            {
                                item.MessageSender = Receiver;
                            }
                        }
                        Data.Add(item);
                        countData++;
                    }
                }
                else
                {
                    if (countData > 0)
                    {
                        lastItem = Data[countData - 1];
                        if (lastItem.SenderId != UserLogged.Id)
                        {

                            Data[countData - 1] = new ChatMessage
                            {
                                Id = lastItem.Id,
                                SenderId = lastItem.SenderId,
                                MessageSender = Receiver,
                                Content = lastItem.Content,
                                ConversationId = lastItem.ConversationId,
                                CreatedOn = lastItem.CreatedOn,
                                ModifiedOn = lastItem.ModifiedOn
                            };
                        }
                        Data.Add(new ChatMessage() { CreatedOn = Data[countData - 1].CreatedOn });
                    }
                    OutOfData = true;
                }
            }
            else
            {
                Data.Clear();
                Page = 1;
            }
            OnPropertyChanged(nameof(IsEmptyList));
        }
        public ICommand ProfileTappedCommand
        {
            get => new Command(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new UserProfile(Receiver.Id));
            });
        }
    }
}