using SimhereApp.Portable.Helpers;
using SimhereApp.Portable.Settings;
using SimhereApp.Portable.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SimhereApp.Portable.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInformationPage : ContentPage
    {
        private readonly UserInformationViewModel viewModel;
        public UserInformationPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new UserInformationViewModel();
        }
    }
}