using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using SimHere.Entities;
using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Models;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.Views;
using Xamarin.Forms;

namespace SimhereApp.Portable.ViewModels
{
    public class ChatConversationViewModel : ListViewPageViewModel<ChatConversationModel>
    {
        public UserLite Sender { get; }
        public ChatConversationViewModel()
        {
            Title = "Chat";
            PreLoadData = new Command(() => { ApiUrl = $"api/chat/conversation?userid={UserLogged.Id}&Page={Page}"; });
            Sender = new UserLite { Id = UserLogged.Id, FullName = UserLogged.FullName, PictureUrl = UserLogged.AvatarUrl };
            Initilize();
        }
        public ICommand ItemTappedCommand
        {
            get
            {
                return new Command<ChatConversationModel>( async (item) =>
                {
                    await Shell.Current.Navigation.PushAsync(new ChatMessagePage(item.Id));
                });
            }
        }
        public async Task Initilize()
        {
            await LoadData();
        }
        public override async Task LoadData()
        {
            PreLoadData.Execute(null);
            var result = await ApiHelper.Get<List<ChatConversation>>(ApiUrl, true);
            if (result.IsSuccess)
            {
                var list = (List<ChatConversation>)result.Content;
                var count = list.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        var item = list[i];
                        var chatConversation = new ChatConversationModel
                        {
                            Id = item.Id,
                            LatestContent = item.LatestContent,
                            CreatedOn = item.CreatedOn,
                            ModifiedOn = item.ModifiedOn,
                        };

                        if (item.User1.Id == UserLogged.Id)
                        {
                            chatConversation.Receiver = item.User2;
                        }
                        else
                        {
                            chatConversation.Receiver = item.User1;
                        }
                        Data.Add(chatConversation);
                    }
                }
                else
                {
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
    }
}

