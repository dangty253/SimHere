using System;
using System.Collections.ObjectModel;
using SimhereApp.Portable.Models;
using Xamarin.Forms;

namespace SimhereApp.Portable.Controls
{
    public partial class PopupMenu : ContentView
    {
        public static BindableProperty IsOpenProperty = BindableProperty.Create(
             nameof(IsOpen)
             , typeof(bool)
             , typeof(PopupMenu)
             , false
             , BindingMode.TwoWay
        );
        public bool IsOpen
        {
            get => (bool)GetValue(IsOpenProperty);
            set
            {
                SetValue(IsOpenProperty, value);
                if (value)
                {
                    AbsoluteLayout.SetLayoutBounds(ControlName, new Rectangle(0, 0, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(ControlName, AbsoluteLayoutFlags.All);
                    MenuIcon.Text = "\uf00d";
                    MenuIcon.TextColor = Color.Black;
                    MenuFrame.BackgroundColor = Color.White;
                    MenuFrame.BorderColor = Color.Black;
                    ControlName.BackgroundColor = Color.FromRgba(255, 255, 255, 0.8);
                    MenuItemsStack.IsVisible = true;
                }
                else
                {
                    AbsoluteLayout.SetLayoutBounds(ControlName, new Rectangle(1, 1, 100, 100));
                    AbsoluteLayout.SetLayoutFlags(ControlName, AbsoluteLayoutFlags.PositionProportional);
                    MenuIcon.Text = "\uf129";
                    MenuIcon.TextColor = Color.White;
                    MenuFrame.BackgroundColor = Color.Blue;
                    MenuFrame.BorderColor = Color.Transparent;
                    ControlName.BackgroundColor = Color.Transparent;
                    MenuItemsStack.IsVisible = false;
                }
            }
        }
       
        private void OnTappedMenuIcon(object sender, EventArgs e)
        {
            IsOpen = !IsOpen;
        }
        private void OnTappedBlank(object sender, EventArgs e)
        {
            if (IsOpen)
            {
                IsOpen = !IsOpen;
            }
        }
        private async void ItemTapped(object sender, EventArgs e)
        {
            var stackLayout = sender as StackLayout;
            var item = (stackLayout.GestureRecognizers[0] as TapGestureRecognizer).CommandParameter as MenuItemModel;
            item.Command.Execute(null);
            IsOpen = !IsOpen;
        }

        public static BindableProperty MenuItemsProperty = BindableProperty.Create(
            nameof(MenuItems)
            , typeof(ObservableCollection<MenuItemModel>)
            , typeof(PopupMenu)
            , null
            , BindingMode.TwoWay
        );
        public ObservableCollection<MenuItemModel> MenuItems
        {
            get => (ObservableCollection<MenuItemModel>)GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public PopupMenu()
        {
            InitializeComponent();
            IsOpen = false;
        }
    }
}
