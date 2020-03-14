using System.Linq;
using SimHere.Entities;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ChatMessagePage : ContentPage
    {
        public ChatMessageViewModel viewModel;
        public ChatMessagePage(string conversationId)
        {
            InitializeComponent();
            BindingContext = viewModel = new ChatMessageViewModel(conversationId);
        }
        public ChatMessagePage(UserLite receiver)
        {
            InitializeComponent();
            BindingContext = viewModel = new ChatMessageViewModel(receiver);
        }
        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (((ChatMessage)e.Item).Id == viewModel.Data.LastOrDefault().Id)
            {
                await viewModel.LoadMoreData();
            }
        }
    }
}
