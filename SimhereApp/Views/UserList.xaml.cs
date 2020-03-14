using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SimhereApp.Portable.Views
{
    public partial class UserList : ContentPage
    {
        public UserList()
        {
            InitializeComponent();
        }
    }

    public class testUser
    {

    }

    public class TestViewModel : ViewModels.ListViewPageViewModel<testUser>
    {
        public TestViewModel()
        {
            PreLoadData = new Command(() => ApiUrl = $"api/testuser");
        }

    }
}
