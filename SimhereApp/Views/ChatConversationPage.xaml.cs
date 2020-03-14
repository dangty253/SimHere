using System;
using System.Collections.Generic;
using SimhereApp.Portable.ViewModels;
using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class ChatConversationPage : ContentPage
    {
        public ChatConversationViewModel viewModel;
        public ChatConversationPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ChatConversationViewModel();
        }
    }
}
